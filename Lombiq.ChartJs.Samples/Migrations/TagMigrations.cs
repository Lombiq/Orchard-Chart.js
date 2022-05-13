using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using static Lombiq.ChartJs.Samples.Constants.ContentTypes;

namespace Lombiq.ChartJs.Samples.Migrations;

public class TagMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public TagMigrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        _contentDefinitionManager.AlterTypeDefinition(Tag, type => type
            .WithPart(nameof(TitlePart))
            .WithPart(nameof(AutoroutePart))
        );

        return 1;
    }
}
