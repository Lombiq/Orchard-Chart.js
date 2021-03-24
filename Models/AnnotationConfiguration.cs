using System.Collections.Generic;

namespace Lombiq.ChartJs.Models
{
    /// <summary>
    /// The main configuration object for placing annotations. Place an instance in the "annotation" property of the
    /// options object on the Chart shape. See more at the plugin's
    /// <see href="https://www.npmjs.com/package/chartjs-plugin-annotation"/>NPM page.
    /// </summary>
    public class AnnotationConfiguration
    {
        public List<Annotation> Annotations { get; } = new();
    }
}
