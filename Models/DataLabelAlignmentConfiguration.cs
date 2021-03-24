using Newtonsoft.Json;
using System;

namespace Lombiq.ChartJs.Models
{
    public class DataLabelAlignmentConfiguration
    {
        [JsonIgnore]
        public DataLabelAlignment Align { get; set; }

        [JsonProperty("align")]
        internal string AlignText
        {
            get => GetAlignment(Align);
            set => Align = SetAlignment(value);
        }

        [JsonIgnore]
        public DataLabelAlignment Anchor { get; set; }

        [JsonProperty("anchor")]
        internal string AnchorText
        {
            get => GetAlignment(Anchor);
            set => Anchor = SetAlignment(value);
        }


        public double Offset { get; set; }

        public FontStyle Font { get; set; }

        private static string GetAlignment(DataLabelAlignment value) =>
            value switch
            {
                DataLabelAlignment.Center => "center",
                DataLabelAlignment.Start => "start",
                DataLabelAlignment.End => "end",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };

        private static DataLabelAlignment SetAlignment(string value) =>
            value switch
            {
                "center" => DataLabelAlignment.Center,
                "start" => DataLabelAlignment.Start,
                "end" => DataLabelAlignment.End,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };

        public class FontStyle
        {
            public double Size { get; set; }

            [JsonIgnore]
            public bool IsBold { get; set; }

            [JsonProperty("weight")]
            public string Weight
            {
                get => IsBold ? "bold" : "normal";
                set => IsBold = value == "bold";
            }
        }
    }
}
