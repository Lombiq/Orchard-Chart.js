using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Settings;
using System.Threading.Tasks;
using YesSql.Sql;

namespace Lombiq.ChartJs.Samples.Helpers;
public static class TransactionMigrationHelpers
{
    public static async Task<int> CreateTransactionAsync<TPart>(
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        ISchemaBuilder schemaBuilder,
        string contentTypeName,
        string typeItemTaxonomyId)
        where TPart : TransactionPart
    {
        var taxonomyTypeDefinition = await contentDefinitionManager.GetTypeDefinitionAsync(ContentTypes.Taxonomy);
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

        await contentDefinitionManager.AlterPartDefinitionAsync<TPart>(part => part
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

        await contentDefinitionManager.AlterTypeDefinitionAsync(contentTypeName, type => type
            .Creatable()
            .Listable()
            .WithPart(typeof(TPart).Name)
        );

        return 1;
    }
}
