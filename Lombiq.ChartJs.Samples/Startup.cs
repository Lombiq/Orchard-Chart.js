using Lombiq.ChartJs.Samples.Indexes;
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
using YesSql.Indexes;

namespace Lombiq.ChartJs.Samples;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDataMigration, TagMigrations>();

        services.AddContentPart<IncomePart>()
            .WithMigration<IncomeMigrations>();

        services.AddContentPart<ExpensePart>()
            .WithMigration<ExpenseMigrations>();

        services.AddSingleton<IIndexProvider, IncomePartIndexProvider>();
        services.AddSingleton<IIndexProvider, ExpensePartIndexProvider>();

        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();

        services.AddScoped<INavigationProvider, ChartJsSamplesNavigationProvider>();
    }
}
