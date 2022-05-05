using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Taxonomies.Settings;
using System;
using System.Threading.Tasks;
using YesSql.Sql;

namespace Lombiq.ChartJs.Samples.Migrations;

public class IncomeMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;

    public IncomeMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
    }

    public async Task<int> CreateAsync()
    {
        var taxonomyTypeDefinition = _contentDefinitionManager.GetTypeDefinition(ContentTypes.Taxonomy);
        var taxonomyItem = await _contentManager.NewAsync(taxonomyTypeDefinition.Name);
        taxonomyItem.DisplayText = ContentTypes.Income + " tags";
        taxonomyItem.ContentItemId = ContentItemIds.IncomeTagsTaxonomy;
        taxonomyItem.Content.TitlePart.Title = ContentTypes.Income + " tags";
        taxonomyItem.Content.TaxonomyPart.TermContentType = ContentTypes.Tag;
        // We need lower case version of content part name here.
#pragma warning disable CA1308 // Normalize strings to uppercase
        taxonomyItem.Content.AliasPart.Alias = ContentTypes.Income.ToLowerInvariant() + "-tags";
        taxonomyItem.Content.AutoroutePart.Path = ContentTypes.Income.ToLowerInvariant() + "-tags";
#pragma warning restore CA1308 // Normalize strings to uppercase
        await _contentManager.CreateAsync(taxonomyItem, VersionOptions.Published);

        _contentDefinitionManager.AlterPartDefinition<IncomePart>(part => part
            .WithField(part => part.Date, field => field
                .WithSettings(new DateFieldSettings { Required = true }))
            .WithField(part => part.Tags, field => field
                .WithSettings(new TaxonomyFieldSettings { TaxonomyContentItemId = taxonomyItem.ContentItemId })
                .WithEditor("Tags")
                .WithDisplayMode("Tags"))
            .WithField(part => part.Description)
            .WithField(part => part.Amount, field => field
                .WithSettings(new NumericFieldSettings
                {
                    Required = true,
                    Minimum = 0,
                }))
        );

        _contentDefinitionManager.AlterTypeDefinition(ContentTypes.Income, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(IncomePart))
        );

        SchemaBuilder.CreateMapIndexTable<IncomePartIndex>(table => table
            .Column<DateTime?>(nameof(IncomePartIndex.Date))
            .Column<string>(nameof(IncomePartIndex.Description))
            .Column<decimal?>(nameof(IncomePartIndex.Amount))
            .Column<string>(nameof(IncomePartIndex.ContentItemId), column => column.WithLength(26))
        );

        SchemaBuilder.AlterTable(nameof(IncomePartIndex), table => table
            .CreateIndex(
            $"IDX_{nameof(IncomePartIndex)}_{nameof(IncomePartIndex.Date)}_{nameof(IncomePartIndex.Amount)}",
            nameof(IncomePartIndex.Date),
            nameof(IncomePartIndex.Amount))
        );

        return 1;
    }
}
