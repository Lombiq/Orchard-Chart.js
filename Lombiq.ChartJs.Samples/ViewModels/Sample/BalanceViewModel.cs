using Lombiq.ChartJs.Models;
using System.Collections.Generic;

namespace Lombiq.ChartJs.Samples.ViewModels.Sample;

public class BalanceViewModel
{
    public IEnumerable<string> Labels { get; set; }
    public IEnumerable<ChartJsDataSet> DataSets { get; set; }
    public object Options { get; set; }
}
