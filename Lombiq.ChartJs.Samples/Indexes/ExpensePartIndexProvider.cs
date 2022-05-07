using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Lombiq.ChartJs.Samples.Indexes;
public class ExpensePartIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context) =>
        context.For<ExpensePartIndex>()
            .Map(contentItem =>
            {
                var expensePart = contentItem.As<ExpensePart>();

                return expensePart == null
                        ? null
                        : new ExpensePartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            Date = expensePart.Date.Value,
                            Amount = expensePart.Amount.Value,
                        };
            });
}
