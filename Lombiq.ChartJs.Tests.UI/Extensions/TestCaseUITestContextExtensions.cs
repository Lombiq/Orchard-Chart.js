using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using SixLabors.ImageSharp;
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

        context.TestChartJsChart("BarChart", "bar_chart", 5);
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        context.TestChartJsChart("LineChart", "line_chart", 10);
    }

    private static void TestChartJsChart(
        this UITestContext context,
        string logHeader,
        string referenceResourceName,
        double pixelErrorThreshold)
    {
        var canvasElementSelector = By.TagName("canvas");
        // This is to avoid Chart.js animation related issues
        var hash = context.DoWaitElementToBeReady(
            canvasElementSelector.Safely(),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMilliseconds(100));
        hash.ShouldNotBeNullOrEmpty();

        context.Scope.AtataContext.Log.Trace("{0}: imageHash: {1}", logHeader, hash);

        using var canvasImage = context.TakeElementScreenshot(canvasElementSelector)
            .ToImageSharpImage();
        canvasImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_canvas.bmp");

        using var referenceImage = GetResourceImageSharpImage($"Lombiq.ChartJs.Tests.UI.Assets.{referenceResourceName}.dib");
        referenceImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_reference.bmp");

        using var diffImage = referenceImage
            .CalcDiffImage(canvasImage);
        diffImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_diff.bmp");

        var diff = referenceImage
            .CompareTo(canvasImage);
        context.Scope.AtataContext.Log.Trace(
            @"{0}: calculated differences:
    absoluteError={1},
    meanError={2},
    pixelErrorCount={3},
    pixelErrorPercentage={4}
            ",
            logHeader,
            diff.AbsoluteError,
            diff.MeanError,
            diff.PixelErrorCount,
            diff.PixelErrorPercentage);

        diff.PixelErrorPercentage.ShouldBeLessThan(pixelErrorThreshold);
    }

    private static Image GetResourceImageSharpImage(string name)
    {
        using var resourceStream = typeof(TestCaseUITestContextExtensions)
            .Assembly
            .GetManifestResourceStream(name);
        return Image.Load(resourceStream);
    }
}
