using Lombiq.ChartJs.Samples.Migrations;
using Lombiq.ChartJs.Samples.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;

namespace Lombiq.ChartJs.Samples;
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentPart<TagPart>()
            .WithMigration<TagMigrations>();

        services.AddContentPart<IncomePart>()
            .WithMigration<IncomeMigrations>();

        services.AddContentPart<ExpensePart>()
            .WithMigration<ExpenseMigrations>();
    }
}
