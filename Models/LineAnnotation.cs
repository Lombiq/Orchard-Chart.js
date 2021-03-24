using Newtonsoft.Json;

namespace Lombiq.ChartJs.Models
{
    public class LineAnnotation : Annotation
    {
        public override string Type => "line";

        [JsonIgnore]
        public bool IsVertical { get; set; }

        public string Mode
        {
            get => IsVertical ? "vertical" : "horizontal";
            set => IsVertical = value == "vertical";
        }

        [JsonProperty("scaleID")]
        public string ScaleId => IsVertical ? "y-axis-0" : "x-axis-0";

        public double Value { get; set; }
    }
}
