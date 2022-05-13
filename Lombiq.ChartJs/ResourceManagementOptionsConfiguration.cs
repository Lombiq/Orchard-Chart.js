using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.ChartJs.Constants.ResourceNames;

namespace Lombiq.ChartJs;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string Vendors = "~/Lombiq.ChartJs/vendors/";

    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Library)
            .SetUrl(Vendors + "chart.js/Chart.min.js", Vendors + "chart.js/Chart.js")
            .SetVersion("2.9.4");

        _manifest
            .DefineScript(Annotation)
            .SetDependencies(Library)
            .SetUrl(
                Vendors + "chartjs-plugin-annotation/chartjs-plugin-annotation.min.js",
                Vendors + "chartjs-plugin-annotation/chartjs-plugin-annotation.js")
            .SetVersion("0.5.7");

        _manifest
            .DefineScript(DataLabels)
            .SetDependencies(Library)
            .SetUrl(
                Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.min.js",
                Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.js")
            .SetVersion("0.7.0");
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
