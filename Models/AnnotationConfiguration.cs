using System.Collections.Generic;

namespace Lombiq.ChartJs.Models
{
    /// <summary>
    /// The main configuration object for placing annotations. Place an instance in the options object on the Chart shape.
    /// </summary>
    public class AnnotationConfiguration
    {
        public List<Annotation> Annotations { get; } = new();
    }
}
