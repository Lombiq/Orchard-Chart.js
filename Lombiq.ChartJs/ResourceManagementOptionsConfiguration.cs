using Lombiq.HelpfulLibraries.Attributes;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.ChartJs.Constants.ResourceNames;

namespace Lombiq.ChartJs;

[LibManVersions]
public partial class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string Vendors = "~/Lombiq.ChartJs/vendors/";

    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Library)
            .SetUrl(Vendors + "chart.js/chart.umd.min.js", Vendors + "chart.js/chart.umd.js")
            .SetVersion(LibManVersions.ChartJs);

        _manifest
            .DefineScript(Annotation)
            .SetDependencies(Library)
            .SetUrl(Vendors + "chartjs-plugin-annotation/chartjs-plugin-annotation.min.js")
            .SetVersion(LibManVersions.ChartjsPluginAnnotation);

        _manifest
            .DefineScript(DataLabels)
            .SetDependencies(Library)
            .SetUrl(
                Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.min.js",
                Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.js")
            .SetVersion(LibManVersions.ChartjsPluginDatalabels);
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
