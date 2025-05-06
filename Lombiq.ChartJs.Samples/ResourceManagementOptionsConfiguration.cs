using Lombiq.ChartJs.Samples.Constants;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Lombiq.ChartJs.Samples;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration() =>
        _manifest
            .DefineStyle(FeatureIds.Area)
            .SetUrl("~/Lombiq.ChartJs.Samples/css/chartjs-samples.css");

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
