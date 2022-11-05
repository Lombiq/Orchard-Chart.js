using Lombiq.ChartJs.Samples.Migrations;
using Lombiq.ChartJs.Samples.Models;
using Lombiq.ChartJs.Samples.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;

namespace Lombiq.ChartJs.Samples;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDataMigration<TagMigrations>();

        services.AddContentPart<IncomePart>()
            .WithMigration<IncomeMigrations>();

        services.AddContentPart<ExpensePart>()
            .WithMigration<ExpenseMigrations>();

        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();

        services.AddScoped<INavigationProvider, ChartJsSamplesNavigationProvider>();
    }
}
