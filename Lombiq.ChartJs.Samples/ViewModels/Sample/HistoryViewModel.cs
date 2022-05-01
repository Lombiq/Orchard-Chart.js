using Lombiq.ChartJs.Models;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Lombiq.ChartJs.Samples.ViewModels.Sample;
public class HistoryViewModel
{
    public IEnumerable<string> Labels { get; set; }
    public IEnumerable<ChartJsDataSet> DataSets { get; set; }
    public object Options { get; set; }
    public IEnumerable<ContentItem> IncomeTerms { get; set; }
    public IEnumerable<ContentItem> ExpenseTerms { get; set; }
    public string IncomeTag { get; set; }
    public string ExpenseTag { get; set; }
}
