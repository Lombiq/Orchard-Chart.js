using System;
using YesSql.Indexes;

namespace Lombiq.ChartJs.Samples.Indexes;

public class TransactionPartIndex : MapIndex
{
    public string ContentItemId { get; set; }
    public DateTime? Date { get; set; }
    public string Description { get; set; }
    public decimal? Amount { get; set; }
}
