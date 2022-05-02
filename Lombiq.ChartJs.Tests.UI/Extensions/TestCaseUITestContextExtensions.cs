using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shouldly;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Tests.UI.Extensions;
public static class TestCaseUITestContextExtensions
{
    private const string LineChartImageHash = "bea329dddacd1c204ea1099a90346bbb92ed93425464bb550dbaa7faa671c611";

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

        var hash = context.WaitChartJsCanvasToBeReadyAndHash();
        context.Scope.AtataContext.Log.Trace($"BarChartImageHashRaw: {hash}");
        var imageDataUrl = context.ComputeElementImageDataUrl(context.Driver.FindElementByTagName("canvas"));
        context.Scope.AtataContext.Log.Trace($"BarChartImage: {imageDataUrl}");
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        var hash = context.WaitChartJsCanvasToBeReadyAndHash();
        context.Scope.AtataContext.Log.Trace($"LineChartImageHashRaw: {hash}");
        var imageDataUrl = context.ComputeElementImageDataUrl(context.Driver.FindElementByTagName("canvas"));
        context.Scope.AtataContext.Log.Trace($"LineChartImage: {imageDataUrl}");
        hash.ShouldBe(LineChartImageHash);
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

    private static string ComputeElementImageDataUrl(this UITestContext context, IWebElement element)
    {
        using var elementImage = context.TakeScreenshotImage(element);
        using var elementImageStream = new MemoryStream();

        elementImage.Save(elementImageStream, ImageFormat.Bmp);
        var elementImageRaw = elementImageStream.ToArray();
        return "data:image/bmp;base64," + Convert.ToBase64String(elementImageRaw);
    }

    private static string WaitChartJsCanvasToBeReadyAndHash(this UITestContext context)
    {
        var wait = new WebDriverWait(context.Driver, timeout: TimeSpan.FromSeconds(30))
        {
            PollingInterval = TimeSpan.FromMilliseconds(100),
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
}
