using Lombiq.ChartJs.Samples.Controllers;
using Lombiq.HelpfulLibraries.OrchardCore.Mvc;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Tests.UI.Extensions;

public static class UITestContextExtensions
{
    public static Task ExecuteChartJsSampleRecipeDirectlyAsync(this UITestContext context) =>
        context.ExecuteRecipeDirectlyAsync("Lombiq.ChartJs.Samples.Content");

    public static Task GoToBalanceAsync(this UITestContext context) =>
        context.GoToAsync<SampleController>(controller => controller.Balance());

    public static Task GoToHistoryAsync(this UITestContext context) =>
        context.GoToAsync<SampleController>(controller => controller.History(null, null));
}
