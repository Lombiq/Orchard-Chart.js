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
    private const string BarChartImageHash = "0da439a0d54840429bc280941774678806eb104c8fc4aa74b5cbee0584e5cfc6";
    private const string LineChartImageHash = "d7837b303c80caf42554fcb47fc37f1290fa83c7de51e49b50a4c6d0fafcd378";

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
        var hash = ComputeSha256Hash(base64Image);

        hash.ShouldBe(BarChartImageHash);
    }

    public static async Task TestChartJsLineChartAsync(this UITestContext context)
    {
        await context.GoToHistoryAsync();

        var base64Image = context.Driver.ExecuteScript(
            "return document.getElementsByTagName('canvas')[0].toDataURL('image/jpg');") as string;
        var hash = ComputeSha256Hash(base64Image);

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
