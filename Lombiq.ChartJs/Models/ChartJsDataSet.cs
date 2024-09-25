using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lombiq.ChartJs.Models;

public class ChartJsDataSet
{
    public string Label { get; set; }

    public IEnumerable<double?> Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string> BackgroundColor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string> BorderColor { get; set; }

    public double BorderWidth { get; set; } = 1;
}
