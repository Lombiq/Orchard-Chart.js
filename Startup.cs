using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace Lombiq.ChartJs
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<IResourceManifestProvider, ResourceManifest>();
    }
}
