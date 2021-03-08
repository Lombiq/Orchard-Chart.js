using Lombiq.HelpfulLibraries.Libraries.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using ResourceManifest = Lombiq.ChartJs.ResourceManifest;

namespace Lombiq.DataTables
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IResourceManifestProvider, ResourceManifest>();
            services.AddOrchardServices();
        }
    }
}
