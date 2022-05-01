using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Tests.UI.Extensions;
public static class TestCaseUITestContextExtensions
{
    private const string BarChartImageHash = "b7168c969791f6a0c84a872e5d525339caceb6b1e933eb27d2ba1a80232a17ac";
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
        context.Scope.AtataContext.Log.Trace($"BarChartImage: {base64Image}");
        var hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"BarChartImageHash: {hash}");

        hash.ShouldBe(BarChartImageHash);
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        var base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg');") as string;
        context.Scope.AtataContext.Log.Trace($"LineChartImage: {base64Image}");
        var hash = ComputeSha256Hash(base64Image);
        context.Scope.AtataContext.Log.Trace($"LineChartImageHash: {hash}");

        hash.ShouldBe(LineChartImageHash);
    }

    private static string ComputeSha256Hash(string raw)
    {
        using var sha256Hash = SHA256.Create();

        return string.Concat(
            sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(raw))
                .Select(item => item.ToString("x2", CultureInfo.InvariantCulture)));
    }
}
