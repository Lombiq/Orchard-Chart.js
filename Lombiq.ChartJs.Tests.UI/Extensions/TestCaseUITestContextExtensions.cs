using Atata;
using Lombiq.HelpfulLibraries.Common.Utilities;
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
        var testDumpFolderName = nameof(TestChartJsSampleBehaviorAsync);
        var testTempRootFolder = FileSystemHelper.EnsureDirectoryExists(Path.Combine("Temp", testDumpFolderName));

        // This is to avoid Chart.js animation related issues
        var hash = context.WaitElementToNotChange(
            canvasElementSelector.Safely(),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMilliseconds(100));
        hash.ShouldNotBeNullOrEmpty();

        context.Scope.AtataContext.Log.Trace("{0}: imageHash: {1}", logHeader, hash);

        using var canvasImage = context.TakeElementScreenshot(canvasElementSelector)
            .ToImageSharpImage();
        var canvasImageTempFile = Path.Combine(testTempRootFolder, $"{logHeader}_canvas.bmp");

        canvasImage.ShouldNotBeNull()
            .SaveAsBmp(canvasImageTempFile);
        context.AppendFailureDump(
            Path.Combine(testDumpFolderName, $"{logHeader}_canvas.bmp"),
            context => Task.FromResult((Stream)File.OpenRead(canvasImageTempFile)));

        using var referenceImage = typeof(TestCaseUITestContextExtensions).Assembly
            .GetResourceImageSharpImage($"Lombiq.ChartJs.Tests.UI.Assets.{referenceResourceName}.dib");
        var referenceImageTempFile = Path.Combine(testTempRootFolder, $"{logHeader}_reference.bmp");

        referenceImage.ShouldNotBeNull()
            .SaveAsBmp(referenceImageTempFile);
        context.AppendFailureDump(
            Path.Combine(testDumpFolderName, $"{logHeader}_reference.bmp"),
            context => Task.FromResult((Stream)File.OpenRead(referenceImageTempFile)));

        using var diffImage = referenceImage
            .CalcDiffImage(canvasImage);
        var diffImageTempFile = Path.Combine(testTempRootFolder, $"{logHeader}_diff.bmp");

        diffImage.ShouldNotBeNull()
            .SaveAsBmp(diffImageTempFile);
        context.AppendFailureDump(
            Path.Combine(testDumpFolderName, $"{logHeader}_diff.bmp"),
            context => Task.FromResult((Stream)File.OpenRead(diffImageTempFile)));

        var diff = referenceImage
            .CompareTo(canvasImage);
        var diffLogTempFile = Path.Combine(testTempRootFolder, $"{logHeader}_diff.log");

        File.WriteAllText(
            diffLogTempFile,
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
            Path.Combine(testDumpFolderName, $"{logHeader}_diff.log"),
            context => Task.FromResult((Stream)File.OpenRead(diffLogTempFile)));

        diff.PixelErrorPercentage.ShouldBeLessThan(pixelErrorThreshold);
    }
}
