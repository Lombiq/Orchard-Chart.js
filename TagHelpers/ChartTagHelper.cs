using Lombiq.ChartJs.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.DisplayManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.TagHelpers
{
    [HtmlTargetElement("chart")]
    public class ChartTagHelper : TagHelper
    {
        private readonly IDisplayHelper _displayHelper;
        private readonly dynamic _shapeFactory;

        [HtmlAttributeName("type")]
        public string ChartType { get; set; } = "bar";

        [HtmlAttributeName("labels")]
        public IEnumerable<string> Labels { get; set; } = Array.Empty<string>();

        [HtmlAttributeName("datasets")]
        public IEnumerable<ChartJsDataSet> DataSets { get; set; } = Array.Empty<ChartJsDataSet>();

        [HtmlAttributeName("options")]
        public object Options { get; set; } = new();

        [HtmlAttributeName("background")]
        public string BackgroundColor { get; set; } = "white";

        [HtmlAttributeName("with-data-labels")]
        public bool WithDataLabels { get; set; }
        public ChartTagHelper(IDisplayHelper displayHelper, IShapeFactory factory)
        {
            _displayHelper = displayHelper;
            _shapeFactory = factory;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var shape = await _shapeFactory.Chart(
                ChartType: ChartType,
                Labels: Labels,
                DataSets: DataSets,
                Options: Options,
                BackgroundColor: BackgroundColor,
                WithDataLabels: WithDataLabels);
            var content = (IHtmlContent)await _displayHelper.ShapeExecuteAsync(shape);

            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PostContent.SetHtmlContent(content);
        }
    }
}
