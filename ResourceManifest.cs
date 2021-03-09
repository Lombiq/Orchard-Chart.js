using OrchardCore.ResourceManagement;
using static Lombiq.ChartJs.Constants.ResourceNames;

namespace Lombiq.ChartJs
{
    public class ResourceManifest : IResourceManifestProvider
    {
        private const string Vendors = "~/Lombiq.ChartJs/vendors/";

        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineScript(Library)
                .SetUrl(Vendors + "chart.js/Chart.min.js", Vendors + "chart.js/Chart.js")
                .SetVersion("2.9.4");
        }
    }
}
