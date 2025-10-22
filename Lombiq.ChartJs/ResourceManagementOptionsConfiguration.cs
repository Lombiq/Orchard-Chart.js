using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.ChartJs.Constants.ResourceNames;

namespace Lombiq.ChartJs;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    public const string ChartJsVersion = "4.5.1";
    public const string ChartJsPluginAnnotationsVersion = "3.1.0";
    public const string ChartJsPluginDataLabelsVersion = "2.2.0";

    private const string Vendors = "~/Lombiq.ChartJs/vendors/";

    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Library)
            .SetUrl(Vendors + "chart.js/dist/chart.umd.min.js", Vendors + "chart.js/dist/chart.umd.js")
            .SetVersion(ChartJsVersion);

        _manifest
            .DefineScript(Annotation)
            .SetDependencies(Library)
            .SetUrl(Vendors + "chartjs-plugin-annotation/dist/chartjs-plugin-annotation.min.js")
            .SetVersion(ChartJsPluginAnnotationsVersion);

        _manifest
            .DefineScript(DataLabels)
            .SetDependencies(Library)
            .SetUrl(
                Vendors + "chartjs-plugin-datalabels/dist/chartjs-plugin-datalabels.min.js",
                Vendors + "chartjs-plugin-datalabels/dist/chartjs-plugin-datalabels.js")
            .SetVersion(ChartJsPluginDataLabelsVersion);
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
