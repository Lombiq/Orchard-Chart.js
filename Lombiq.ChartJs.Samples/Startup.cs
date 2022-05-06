using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.Migrations;
using Lombiq.ChartJs.Samples.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using YesSql.Indexes;

namespace Lombiq.ChartJs.Samples;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentPart<TransactionPart>()
            .WithMigration<TagMigrations>();

        services.AddContentPart<IncomePart>()
            .WithMigration<IncomeMigrations>();

        services.AddContentPart<ExpensePart>()
            .WithMigration<ExpenseMigrations>();

        services.AddSingleton<IIndexProvider, IncomePartIndexProvider>();
        services.AddSingleton<IIndexProvider, ExpensePartIndexProvider>();

        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
    }
}
