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

            manifest
                .DefineScript(Annotation)
                .SetDependencies(Library)
                .SetUrl(
                    Vendors + "chartjs-plugin-annotation/chartjs-plugin-annotation.min.js",
                    Vendors + "chartjs-plugin-annotation/chartjs-plugin-annotation.js")
                .SetVersion("0.5.7");

            manifest
                .DefineScript(DataLabels)
                .SetDependencies(Library)
                .SetUrl(
                    Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.min.js",
                    Vendors + "chartjs-plugin-datalabels/chartjs-plugin-datalabels.js")
                .SetVersion("0.7.0");
        }
    }
}
