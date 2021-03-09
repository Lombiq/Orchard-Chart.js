using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lombiq.ChartJs.Models
{
    public class ChartJsDataSet
    {
        public string Label { get; set; }

        public IEnumerable<double?> Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> BackgroundColor { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> BorderColor { get; set; }

        public double BorderWidth { get; set; } = 1;
    }
}
