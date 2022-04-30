using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;

namespace Lombiq.ChartJs.Samples.Models;

public class ExpensePart : ContentPart
{
    public DateField Date { get; set; } = new();
    public TaxonomyField Tags { get; set; } = new();
    public TextField Description { get; set; } = new();
    public NumericField Amount { get; set; } = new();
}
