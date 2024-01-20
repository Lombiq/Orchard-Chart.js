using Lombiq.ChartJs.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.DisplayManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.TagHelpers;

[HtmlTargetElement("chart")]
public class ChartTagHelper(IDisplayHelper displayHelper, IShapeFactory factory) : TagHelper
{
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

    [HtmlAttributeName("datalabels")]
    public DataLabelConfiguration DataLabelConfiguration { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        IShape shape = await factory.New.Chart(
            ChartType: ChartType,
            Labels: Labels,
            DataSets: DataSets,
            Options: Options,
            BackgroundColor: BackgroundColor,
            DataLabelConfiguration: DataLabelConfiguration);
        var content = await displayHelper.ShapeExecuteAsync(shape);

        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        output.PostContent.SetHtmlContent(content);
    }
}
