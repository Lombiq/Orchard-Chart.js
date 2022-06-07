using Atata;
using Codeuctivity.ImageSharpCompare;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using SixLabors.ImageSharp;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

// We can't import the whole System.Drawing namespace because both of SixLabors.ImageSharp and System.Drawing are
// contains Image class, but we want it and some other from SixLabors.ImageSharp.
// So we import only Bitmap from System.Drawing here.
using Bitmap = System.Drawing.Bitmap;

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
        // This is to avoid Chart.js animation related issues
        var hash = context.WaitChartJsCanvasToBeReadyAndHash();
        hash.ShouldNotBeNullOrEmpty();

        context.Scope.AtataContext.Log.Trace("{0}: imageHash: {1}", logHeader, hash);

        var canvas = context.Get(By.TagName("canvas"));
        canvas.ShouldNotBeNull();
        using var canvasImage = context.TakeScreenshotImage(canvas)
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

        using var diffImage = ImageSharpCompare.CalcDiffMaskImage(
            canvasImageTempFileName,
            referenceImageTempFileName);
        var diffImageTempFileName = $"Temp/{logHeader}_diff.bmp";

        diffImage.ShouldNotBeNull()
            .SaveAsBmp(diffImageTempFileName);
        context.AppendFailureDump(
            $"{logHeader}_diff.bmp",
            context => Task.FromResult((Stream)File.OpenRead(diffImageTempFileName)));

        var diff = ImageSharpCompare.CalcDiff(
            canvasImageTempFileName,
            referenceImageTempFileName);
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

    private static string WaitChartJsCanvasToBeReadyAndHash(
        this UITestContext context,
        double timeoutSec = 30,
        double pollMillisec = 100)
    {
        string lastHash = null;
        context.DoWithRetriesOrFail(
            () =>
            {
                var canvas = context.Get(By.TagName("canvas").Safely());
                var hash = context.ComputeElementImageHash(canvas);

                var ready = hash == lastHash;

                lastHash = hash;

                return ready;
            },
            TimeSpan.FromSeconds(timeoutSec),
            TimeSpan.FromMilliseconds(pollMillisec));
        return lastHash;
    }

    private static string ComputeElementImageHash(this UITestContext context, IWebElement element)
    {
        using var elementImage = context.TakeScreenshotImage(element);
        using var elementImageStream = new MemoryStream();

        elementImage.Save(elementImageStream, ImageFormat.Bmp);
        var elementImageRaw = elementImageStream.ToArray();
        return ComputeSha256Hash(elementImageRaw);
    }

    private static string ComputeSha256Hash(byte[] raw)
    {
        using var sha256Hash = SHA256.Create();

        return string.Concat(
            sha256Hash.ComputeHash(raw)
                .Select(item => item.ToString("x2", CultureInfo.InvariantCulture)));
    }

    private static Image GetResourceImageSharpImage(string name)
    {
        using var resourceStream = typeof(TestCaseUITestContextExtensions)
            .Assembly
            .GetManifestResourceStream(name);
        return Image.Load(resourceStream);
    }

    private static Image ToImageSharpImage(this Bitmap bitmap)
    {
        using var memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, ImageFormat.Bmp);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return Image.Load(memoryStream);
    }
}
