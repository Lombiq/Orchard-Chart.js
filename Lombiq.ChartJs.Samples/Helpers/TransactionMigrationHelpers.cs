using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Settings;
using System;
using System.Threading.Tasks;
using YesSql.Sql;

namespace Lombiq.ChartJs.Samples.Helpers;
public static class TransactionMigrationHelpers
{
    public static async Task<int> CreateTransactionAsync<TPart, TPartIndex>(
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        ISchemaBuilder schemaBuilder,
        string contentTypeName,
        string typeItemTaxonomyId)
        where TPart : TransactionPart
        where TPartIndex : TransactionPartIndex
    {
        var taxonomyTypeDefinition = contentDefinitionManager.GetTypeDefinition(ContentTypes.Taxonomy);
        var taxonomyItem = await contentManager.NewAsync(taxonomyTypeDefinition.Name);
        taxonomyItem.DisplayText = contentTypeName + " tags";
        taxonomyItem.ContentItemId = typeItemTaxonomyId;
        taxonomyItem.Content.TitlePart.Title = contentTypeName + " tags";
        // We need lower case version of content type name here
#pragma warning disable CA1308 // Normalize strings to uppercase
        taxonomyItem.Content.AliasPart.Alias = contentTypeName.ToLowerInvariant() + "-tags";
        taxonomyItem.Content.AutoroutePart.Path = contentTypeName.ToLowerInvariant() + "-tags";
#pragma warning restore CA1308 // Normalize strings to uppercase
        taxonomyItem.Content.TaxonomyPart.TermContentType = ContentTypes.Tag;
        await contentManager.CreateAsync(taxonomyItem, VersionOptions.Published);

        contentDefinitionManager.AlterPartDefinition<TPart>(part => part
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

        contentDefinitionManager.AlterTypeDefinition(contentTypeName, type => type
            .Creatable()
            .Listable()
            .WithPart(typeof(TPart).Name)
        );

        schemaBuilder.CreateMapIndexTable<TPartIndex>(table => table
            .Column<DateTime?>(nameof(TransactionPartIndex.Date))
            .Column<decimal?>(nameof(TransactionPartIndex.Amount))
            .Column<string>(nameof(TransactionPartIndex.ContentItemId), column => column.WithLength(26))
        );

        schemaBuilder.AlterTable(typeof(TPartIndex).Name, table => table
            .CreateIndex(
            $"IDX_{typeof(TPartIndex).Name}_{nameof(TransactionPartIndex.Date)}_{nameof(TransactionPartIndex.Amount)}",
            nameof(TransactionPartIndex.Date),
            nameof(TransactionPartIndex.Amount))
        );

        return 1;
    }
}
