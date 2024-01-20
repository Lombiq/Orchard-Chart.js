using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using System.Threading.Tasks;
using static Lombiq.ChartJs.Samples.Constants.ContentTypes;

namespace Lombiq.ChartJs.Samples.Migrations;

public class TagMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(Tag, type => type
            .WithPart(nameof(TitlePart))
            .WithPart(nameof(AutoroutePart))
        );

        return 1;
    }
}
