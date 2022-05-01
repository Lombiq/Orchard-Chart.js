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
public class ExpenseMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;

    public ExpenseMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
    }

    public async Task<int> CreateAsync()
    {
        var taxonomyTypeDefinition = _contentDefinitionManager.GetTypeDefinition(ContentTypes.Taxonomy);
        var taxonomyItem = await _contentManager.NewAsync(taxonomyTypeDefinition.Name);
        taxonomyItem.DisplayText = ContentTypes.Expense + " tags";
        taxonomyItem.ContentItemId = ContentItemIds.ExpenseTagsTaxonomy;
        taxonomyItem.Content.TitlePart.Title = ContentTypes.Expense + " tags";
#pragma warning disable CA1308 // Normalize strings to uppercase
        taxonomyItem.Content.AliasPart.Alias = ContentTypes.Expense.ToLowerInvariant() + "-tags";
        taxonomyItem.Content.AutoroutePart.Path = ContentTypes.Expense.ToLowerInvariant() + "-tags";
#pragma warning restore CA1308 // Normalize strings to uppercase
        taxonomyItem.Content.TaxonomyPart.TermContentType = ContentTypes.Tag;
        await _contentManager.CreateAsync(taxonomyItem, VersionOptions.Published);

        _contentDefinitionManager.AlterPartDefinition<ExpensePart>(part => part
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

        _contentDefinitionManager.AlterTypeDefinition(ContentTypes.Expense, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(ExpensePart))
        );

        SchemaBuilder.CreateMapIndexTable<ExpensePartIndex>(table => table
            .Column<DateTime?>(nameof(ExpensePartIndex.Date))
            .Column<string>(nameof(ExpensePartIndex.Description))
            .Column<decimal?>(nameof(ExpensePartIndex.Amount))
            .Column<string>(nameof(ExpensePartIndex.ContentItemId), column => column.WithLength(26))
        );

        SchemaBuilder.AlterTable(nameof(ExpensePartIndex), table => table
            .CreateIndex(
            $"IDX_{nameof(ExpensePartIndex)}_{nameof(ExpensePartIndex.Date)}_{nameof(ExpensePartIndex.Amount)}",
            nameof(ExpensePartIndex.Date),
            nameof(ExpensePartIndex.Amount))
        );

        return 2;
    }

    public int UpdateFrom1()
    {
        SchemaBuilder.CreateMapIndexTable<ExpensePartIndex>(table => table
            .Column<DateTime?>(nameof(ExpensePartIndex.Date))
            .Column<string>(nameof(ExpensePartIndex.Description))
            .Column<decimal?>(nameof(ExpensePartIndex.Amount))
            .Column<string>(nameof(ExpensePartIndex.ContentItemId), column => column.WithLength(26))
        );

        SchemaBuilder.AlterTable(nameof(ExpensePartIndex), table => table
            .CreateIndex(
            $"IDX_{nameof(ExpensePartIndex)}_{nameof(ExpensePartIndex.Date)}_{nameof(ExpensePartIndex.Amount)}",
            nameof(ExpensePartIndex.Date),
            nameof(ExpensePartIndex.Amount))
        );

        return 2;
    }
}
