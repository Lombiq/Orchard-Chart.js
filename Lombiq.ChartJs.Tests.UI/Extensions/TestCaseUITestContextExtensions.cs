using Codeuctivity.ImageSharpCompare;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shouldly;
using SixLabors.ImageSharp;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
// This is because both of SixLabors.ImageSharp and System.Drawing are contains Image class,
// but we want it from SixLabors.ImageSharp
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
        context.Scope.AtataContext.Log.Trace($"{logHeader}: imageHash: {hash}");
        var canvas = context.Driver.FindElementByTagName("canvas");
        canvas.ShouldNotBeNull();
        using var canvasImage = context.TakeScreenshotImage(canvas)
            .ToImageSharpImage();
        canvasImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_canvas.bmp");
        using var referenceImage = GetResourceImageSharpImage($"Lombiq.ChartJs.Tests.UI.Assets.{referenceResourceName}.dib");
        referenceImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_reference.bmp");
        using var diffImage = ImageSharpCompare.CalcDiffMaskImage(canvasImage, referenceImage);
        diffImage.ShouldNotBeNull()
            .SaveAsBmp($"Temp/{logHeader}_diff.bmp");
        var diff = ImageSharpCompare.CalcDiff(canvasImage, referenceImage);
        context.Scope.AtataContext.Log.Trace($@"{logHeader}: diff:
    absoluteError={diff.AbsoluteError.ToString(CultureInfo.InvariantCulture)},
    meanError={diff.MeanError.ToString(CultureInfo.InvariantCulture)},
    pixelErrorCount={diff.PixelErrorCount.ToString(CultureInfo.InvariantCulture)},
    pixelErrorPercentage={diff.PixelErrorPercentage.ToString(CultureInfo.InvariantCulture)}
");
        diff.PixelErrorPercentage.ShouldBeLessThan(pixelErrorThreshold);
    }

    private static string ComputeSha256Hash(byte[] raw)
    {
        using var sha256Hash = SHA256.Create();

        return string.Concat(
            sha256Hash.ComputeHash(raw)
                .Select(item => item.ToString("x2", CultureInfo.InvariantCulture)));
    }

    private static string ComputeElementImageHash(this UITestContext context, IWebElement element)
    {
        using var elementImage = context.TakeScreenshotImage(element);
        using var elementImageStream = new MemoryStream();

        elementImage.Save(elementImageStream, ImageFormat.Bmp);
        var elementImageRaw = elementImageStream.ToArray();
        return ComputeSha256Hash(elementImageRaw);
    }

    private static string WaitChartJsCanvasToBeReadyAndHash(
        this UITestContext context,
        double timeoutSec = 30,
        double pollMillisec = 100)
    {
        var wait = new WebDriverWait(context.Driver, timeout: TimeSpan.FromSeconds(timeoutSec))
        {
            PollingInterval = TimeSpan.FromMilliseconds(pollMillisec),
        };
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

        string lastHash = null;
        return wait.Until((_) =>
        {
            var canvas = context.Driver.FindElementByTagName("canvas");
            var hash = context.ComputeElementImageHash(canvas);
            if (string.IsNullOrEmpty(lastHash))
            {
                context.Scope.AtataContext.Log.Trace($"WaitChartJsCanvasToBeReadyAndHash: lastHash is null or empty");
                lastHash = hash;
                return null;
            }

            if (hash != lastHash)
            {
                context.Scope.AtataContext.Log.Trace($"WaitChartJsCanvasToBeReadyAndHash: lastHash({lastHash}) != hash({hash})");
                return null;
            }

            return hash;
        });
    }

    private static Image GetResourceImageSharpImage(string name)
    {
        using var resourceStream = typeof(TestCaseUITestContextExtensions).Assembly
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
