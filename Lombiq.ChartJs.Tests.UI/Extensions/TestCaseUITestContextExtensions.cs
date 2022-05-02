using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Tests.UI.Extensions;
public static class TestCaseUITestContextExtensions
{
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable CA1823 // Avoid unused private fields
    private const string BarChartImageHash = "b7168c969791f6a0c84a872e5d525339caceb6b1e933eb27d2ba1a80232a17ac";
#pragma warning restore CA1823 // Avoid unused private fields
#pragma warning restore S1144 // Unused private types or members should be removed
    private const string LineChartImageHash = "520975998df620bfa4f5b0c7a2bf2f107a8b1b50869cbe5118931562fd0830e9";

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

        var base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg');") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(jpg): {base64Image}");
        var hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(jpg): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg', 0.1);") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(jpg, 0.1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(jpg, 0.1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg', 1);") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(jpg, 1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(jpg, 1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png');") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(png): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(png): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png', 0.1);") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(png, 0.1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(png, 0.1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png', 1);") as string;
        context.Scope.AtataContext.Log.Trace($"BarChartImage(png, 1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash(png, 1): {hash}");

        var canvas = context.Driver.FindElementByTagName("canvas");
        using var canvasImage = context.TakeScreenshotImage(canvas);
        using var canvasImageStream = new MemoryStream();

        canvasImage.Save(canvasImageStream, ImageFormat.Bmp);
        var canvasImageRaw = canvasImageStream.ToArray();
        base64Image = Convert.ToBase64String(canvasImageRaw);
        hash = ComputeSha256Hash(canvasImageRaw);
        context.Scope.AtataContext.Log.Trace($"BarChartImageRaw(bmp): {base64Image}");
        context.Scope.AtataContext.Log.Trace($"BarChartImageHashRaw(bmp): {hash}");
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        var base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg');") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(jpg): {base64Image}");
        var hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(jpg): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg', 0.1);") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(jpg, 0.1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(jpg, 0.1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg', 1);") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(jpg, 1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(jpg, 1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png');") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(png): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(png): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png', 0.1);") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(png, 0.1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(png, 0.1): {hash}");

        base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/png', 1);") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage(png, 1): {base64Image}");
        hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash(png, 1): {hash}");

        var canvas = context.Driver.FindElementByTagName("canvas");
        using var canvasImage = context.TakeScreenshotImage(canvas);
        using var canvasImageStream = new MemoryStream();

        canvasImage.Save(canvasImageStream, ImageFormat.Bmp);
        var canvasImageRaw = canvasImageStream.ToArray();
        base64Image = Convert.ToBase64String(canvasImageRaw);
        hash = ComputeSha256Hash(canvasImageRaw);
        context.Scope.AtataContext.Log.Trace($"LineChartImageRaw(bmp): {base64Image}");
        context.Scope.AtataContext.Log.Trace($"LineChartImageHashRaw(bmp): {hash}");

        hash.ShouldBe(LineChartImageHash);
    }

    private static string ComputeSha256Hash(string raw)
    {
        using var sha256Hash = SHA256.Create();

        return string.Concat(
            sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(raw))
                .Select(item => item.ToString("x2", CultureInfo.InvariantCulture)));
    }

    private static string ComputeSha256Hash(byte[] raw)
    {
        using var sha256Hash = SHA256.Create();

        return string.Concat(
            sha256Hash.ComputeHash(raw)
                .Select(item => item.ToString("x2", CultureInfo.InvariantCulture)));
    }
}
