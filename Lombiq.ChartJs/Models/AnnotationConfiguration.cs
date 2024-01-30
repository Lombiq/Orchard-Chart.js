using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.ChartJs.Models;

/// <summary>
/// The main configuration object for placing annotations. Place an instance in the "annotation" property of the options
/// object on the Chart shape. See more at the plugin's <see
/// href="https://www.npmjs.com/package/chartjs-plugin-annotation"/> NPM page.
/// </summary>
public class AnnotationConfiguration
{
    [SuppressMessage(
        "Design",
        "MA0016:Prefer return collection abstraction instead of implementation",
        Justification = "Needs to be modifiable for configuration editing.")]
    public List<Annotation> Annotations { get; } = [];
}
