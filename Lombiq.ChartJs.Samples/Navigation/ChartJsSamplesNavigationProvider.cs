using Lombiq.ChartJs.Samples.Controllers;
using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.ChartJs.Samples.Navigation;

public class ChartJsSamplesNavigationProvider(
    IHttpContextAccessor hca,
    IStringLocalizer<ChartJsSamplesNavigationProvider> stringLocalizer) : MainMenuNavigationProviderBase(hca, stringLocalizer)
{
    protected override void Build(NavigationBuilder builder)
    {
        var context = _hca.HttpContext;
        builder
            .Add(T["ChartJs Samples"], builder => builder
                .Add(T["Balance"], itemBuilder => itemBuilder
                    .ActionTask<SampleController>(context, controller => controller.Balance()))
                .Add(T["History"], itemBuilder => itemBuilder
                    .ActionTask<SampleController>(context, controller => controller.History(null, null))));
    }
}
