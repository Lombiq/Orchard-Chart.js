using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Taxonomies.Settings;
using System.Threading.Tasks;
using static Lombiq.ChartJs.Samples.Constants.ContentTypes;

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
        var taxonomyTypeDefinition = _contentDefinitionManager.GetTypeDefinition(Taxonomy);
        var taxonomyItem = await _contentManager.NewAsync(taxonomyTypeDefinition.Name);
        taxonomyItem.DisplayText = Expense + " tags";
        taxonomyItem.Content.TitlePart.Title = Expense + " tags";
        taxonomyItem.Content.TaxonomyPart.TermContentType = Tag;
        await _contentManager.CreateAsync(taxonomyItem, VersionOptions.Published);

        _contentDefinitionManager.AlterPartDefinition<ExpensePart>(part => part
            .WithField(part => part.Date)
            .WithField(part => part.Tags, field => field
                .WithSettings(new TaxonomyFieldSettings { TaxonomyContentItemId = taxonomyItem.ContentItemId })
                .WithEditor("Tags")
                .WithDisplayMode("Tags"))
            .WithField(part => part.Description)
            .WithField(part => part.Amount)
        );

        _contentDefinitionManager.AlterTypeDefinition(Expense, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(ExpensePart))
        );

        return 1;
    }
}
