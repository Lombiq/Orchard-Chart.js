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
            .SetUrl(Vendors + "chart.js/dist/chart.umd.min.js", Vendors + "chart.js/dist/chart.umd.js")
            .SetVersion(LibManVersions.ChartJs);

        _manifest
            .DefineScript(Annotation)
            .SetDependencies(Library)
            .SetUrl(Vendors + "chartjs-plugin-annotation/dist/chartjs-plugin-annotation.min.js")
            .SetVersion(LibManVersions.ChartjsPluginAnnotation);

        _manifest
            .DefineScript(DataLabels)
            .SetDependencies(Library)
            .SetUrl(
                Vendors + "chartjs-plugin-datalabels/dist/chartjs-plugin-datalabels.min.js",
                Vendors + "chartjs-plugin-datalabels/dist/chartjs-plugin-datalabels.js")
            .SetVersion(LibManVersions.ChartjsPluginDatalabels);
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
