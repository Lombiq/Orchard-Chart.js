using OrchardCore.Modules;
using System.Text.Json.Serialization;

namespace Lombiq.ChartJs.Models;

public class LineAnnotation : Annotation
{
    public override string Type => "line";

    [JsonIgnore]
    public bool IsVertical { get; set; }

    public string Mode
    {
        get => IsVertical ? "vertical" : "horizontal";
        set => IsVertical = value.EqualsOrdinalIgnoreCase("vertical");
    }

    [JsonPropertyName("scaleID")]
    public string ScaleId => IsVertical ? "x-axis-0" : "y-axis-0";

    public double Value { get; set; }
}
