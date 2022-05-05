using Lombiq.ChartJs.Models;
using System.Collections.Generic;

namespace Lombiq.ChartJs.Samples.ViewModels.Sample;
public class HistoryViewModel
{
    public IEnumerable<string> Labels { get; set; }
    public IEnumerable<ChartJsDataSet> DataSets { get; set; }
    public object Options { get; set; }
    // We need IncomeTerms and ExpenseTerms to transfer data from controller to view.
#pragma warning disable CA2227 // Collection properties should be read only
    public IDictionary<string, string> IncomeTerms { get; set; }
    public IDictionary<string, string> ExpenseTerms { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    public string IncomeTag { get; set; }
    public string ExpenseTag { get; set; }
}
