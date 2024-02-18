using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Tests.UI.Extensions;

public static class TestCaseUITestContextExtensions
{
    public static async Task TestChartJsSampleBehaviorAsync(this UITestContext context)
    {
        await context.SignInDirectlyAsync();
        await context.ExecuteChartJsSampleRecipeDirectlyAsync();
        await context.TestChartJsBarChartAsync();
        await context.TestChartJsLineChartAsync();
    }

    public static async Task TestChartJsBarChartAsync(this UITestContext context)
    {
        await context.GoToBalanceAsync();

        context.TestChartJsChart("BarChart", 2);
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        context.TestChartJsChart("LineChart", 5);
    }

    private static void TestChartJsChart(
        this UITestContext context,
        string logHeader,
        double pixelErrorPercentageThreshold)
    {
        var canvasElementSelector = By.TagName("canvas");

        // This is to avoid Chart.js animation-related issues.
        var hash = context.WaitElementToNotChange(
            canvasElementSelector.Safely(),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMilliseconds(100));
        hash.ShouldNotBeNullOrEmpty();

        context.Scope.AtataContext.Log.Trace($"{logHeader}: imageHash: {hash}");

        context.AssertVisualVerificationApproved(
            canvasElementSelector,
            pixelErrorPercentageThreshold,
            configurator: configuration => configuration
                .WithCallerLocation());
    }
}
