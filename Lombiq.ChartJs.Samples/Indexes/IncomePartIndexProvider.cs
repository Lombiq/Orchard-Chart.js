using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Lombiq.ChartJs.Samples.Indexes;
public class IncomePartIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context) =>
        context.For<IncomePartIndex>()
            .Map(contentItem =>
            {
                var incomePart = contentItem.As<IncomePart>();

                return incomePart == null
                        ? null
                        : new IncomePartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            Date = incomePart.Date.Value,
                            Amount = incomePart.Amount.Value,
                        };
            });
}
