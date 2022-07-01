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
    public static async Task TestChartJsSampleBehaviorAsync(this UITestContext context, By stickyHeader = null)
    {
        //await context.SelectThemeAsync("TheTheme");
        //var stickyHeaderLocal = By.TagName("nav");

        await context.SignInDirectlyAsync();
        await context.ExecuteChartJsSampleRecipeDirectlyAsync();
        await context.TestChartJsBarChartAsync(stickyHeader);
        await context.TestChartJsLineChartAsync(stickyHeader);
    }

    public static async Task TestChartJsBarChartAsync(this UITestContext context, By stickyHeader = null)
    {
        await context.GoToBalanceAsync();

        context.TestChartJsChart("BarChart", 4, stickyHeader);
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context, By stickyHeader = null)
    {
        await context.GoToHistoryAsync();

        context.TestChartJsChart("LineChart", 10, stickyHeader);
    }

    private static void TestChartJsChart(
        this UITestContext context,
        string logHeader,
        double pixelErrorPercentageThreshold,
        By stickyHeader = null)
    {
        var canvasElementSelector = By.TagName("canvas");

        if (stickyHeader != null)
        {
            context.SetElementStyle(stickyHeader, "position", "absolute");
        }

        // This is to avoid Chart.js animation-related issues.
        var hash = context.WaitElementToNotChange(
            canvasElementSelector.Safely(),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMilliseconds(100));
        hash.ShouldNotBeNullOrEmpty();

        context.Scope.AtataContext.Log.Trace("{0}: imageHash: {1}", logHeader, hash);

        context.AssertVisualVerificationApproved(
            canvasElementSelector,
            pixelErrorPercentageThreshold,
            configurator: configuration => configuration
                .WithCallerLocation());
    }
}
