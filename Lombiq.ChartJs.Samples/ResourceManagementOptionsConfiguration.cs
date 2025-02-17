using Microsoft.Extensions.Options;
using OrchardCore.Modules.Manifest;
using OrchardCore.ResourceManagement;
using System.Linq;
using System.Reflection;

namespace Lombiq.ChartJs.Samples;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        var moduleVersion = typeof(ResourceManagementOptionsConfiguration)
            .Assembly
            .GetCustomAttributes<ModuleAttribute>()
            .First()
            .Version;

        _manifest
            .DefineStyle("Lombiq.ChartJs.Samples")
            .SetUrl(
                "~/Lombiq.ChartJs.Samples/css/chartjs-samples.min.css",
                "~/Lombiq.ChartJs.Samples/css/chartjs-samples.css")
            .SetVersion(moduleVersion);
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
