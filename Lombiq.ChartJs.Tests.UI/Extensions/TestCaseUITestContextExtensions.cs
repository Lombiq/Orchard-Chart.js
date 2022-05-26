using Atata;
using Codeuctivity;
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

// The SixLabors.ImageSharp.Web v2.0.0 which is depends on SixLabors.ImageSharp v2.1.1 will be introduced in Orchard
// in the future.
// See: https://github.com/OrchardCMS/OrchardCore/pull/11585
// When it will be done and we change the new version of OC in Lombiq.ChartJs.Samples, we should change from
// ImageSharpCompare v1.2.11 to Codeuctivity.ImageSharpCompare v2.0.46 in this project

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
        canvasImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_canvas.bmp");

        using var referenceImage = GetResourceImageSharpImage($"Lombiq.ChartJs.Tests.UI.Assets.{referenceResourceName}.dib");
        referenceImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_reference.bmp");

        using var diffImage = ImageSharpCompare.CalcDiffMaskImage(
            $"Temp/{logHeader}_canvas.bmp",
            $"Temp/{logHeader}_reference.bmp");
        diffImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_diff.bmp");

        var diff = ImageSharpCompare.CalcDiff(
            $"Temp/{logHeader}_canvas.bmp",
            $"Temp/{logHeader}_reference.bmp");
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
