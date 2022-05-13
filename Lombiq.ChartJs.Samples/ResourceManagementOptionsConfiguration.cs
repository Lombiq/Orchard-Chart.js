using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Lombiq.ChartJs.Samples;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration() =>
        _manifest
            .DefineStyle("Lombiq.ChartJs.Samples")
            .SetUrl(
                "~/Lombiq.ChartJs.Samples/css/chartjs-samples.min.css",
                "~/Lombiq.ChartJs.Samples/css/chartjs-samples.css")
            .SetVersion("1.0.0");

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
