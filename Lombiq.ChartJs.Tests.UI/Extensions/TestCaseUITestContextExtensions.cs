using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using SixLabors.ImageSharp;
using System;
using System.Globalization;
using System.IO;
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
        var canvasImageTempFileName = $"Temp/{logHeader}_canvas.bmp";

        canvasImage.ShouldNotBeNull()
            .SaveAsBmp(canvasImageTempFileName);
        context.AppendFailureDump(
            $"{logHeader}_canvas.bmp",
            context => Task.FromResult((Stream)File.OpenRead(canvasImageTempFileName)));

        using var referenceImage = GetResourceImageSharpImage(
            $"Lombiq.ChartJs.Tests.UI.Assets.{referenceResourceName}.dib");
        var referenceImageTempFileName = $"Temp/{logHeader}_reference.bmp";

        referenceImage.ShouldNotBeNull()
            .SaveAsBmp(referenceImageTempFileName);
        context.AppendFailureDump(
            $"{logHeader}_reference.bmp",
            context => Task.FromResult((Stream)File.OpenRead(referenceImageTempFileName)));

        using var diffImage = referenceImage
            .CalcDiffImage(canvasImage);
        var diffImageTempFileName = $"Temp/{logHeader}_diff.bmp";

        diffImage.ShouldNotBeNull()
            .SaveAsBmp(diffImageTempFileName);
        context.AppendFailureDump(
            $"{logHeader}_diff.bmp",
            context => Task.FromResult((Stream)File.OpenRead(diffImageTempFileName)));

        var diff = referenceImage
            .CompareTo(canvasImage);
        var diffLogTempFileName = $"Temp/{logHeader}_diff.log";

        File.WriteAllText(
            diffLogTempFileName,
            string.Format(
                CultureInfo.InvariantCulture,
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
                diff.PixelErrorPercentage));
        context.AppendFailureDump(
            $"{logHeader}_diff.log",
            context => Task.FromResult((Stream)File.OpenRead(diffLogTempFileName)));

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
