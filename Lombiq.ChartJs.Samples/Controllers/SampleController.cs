using Lombiq.ChartJs.Constants;
using Lombiq.ChartJs.Models;
using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Models;
using Lombiq.ChartJs.Samples.ViewModels.Sample;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.ChartJs.Samples.Controllers;

public class SampleController : Controller
{
    private readonly IOrchardHelper _orchardHelper;
    private readonly ISession _session;
    private readonly IContentManager _contentManager;

    public SampleController(
        IOrchardHelper orchardHelper,
        ISession session,
        IContentManager contentManager)
    {
        _orchardHelper = orchardHelper;
        _session = session;
        _contentManager = contentManager;
    }

    // Generates a view(Views/Balance.cshtml) with bar chart using <chart> tag helper with two bars to display the actual balance.
    // It uses data from Income and Expense content types stored in db as dataset to it.
    // Lombiq.ChartJs.Samples/Sample/Balance
    public async Task<IActionResult> Balance() =>
        View(new BalanceViewModel
        {
            Labels = new[] { Labels.Balance },
            DataSets = new[]
            {
                // You can find more detailed description about dataset here:
                // https://www.chartjs.org/docs/2.9.4/charts/bar.html#dataset-properties
                new ChartJsDataSet
                {
                    Label = Labels.Incomes,
                    BackgroundColor = new[] { ChartColors.IncomesBarChartBackgroundColor },
                    Data = new double?[]
                    {
                        (await _session.QueryIndex<NumericFieldIndex>(
                            index =>
                                index.Published &&
                                index.Latest &&
                                index.ContentType == ContentTypes.Income &&
                                index.ContentPart == nameof(IncomePart) &&
                                index.ContentField == nameof(IncomePart.Amount) &&
                                index.Numeric != null)
                            .ListAsync())
                        .Select(index => decimal.ToDouble(index.Numeric ?? 0m))
                        .Sum(),
                    },
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { ChartColors.ExpensesBarChartBackgroundColor },
                    Data = new double?[]
                    {
                        (await _session.QueryIndex<NumericFieldIndex>(
                            index =>
                                index.Published &&
                                index.Latest &&
                                index.ContentType == ContentTypes.Expense &&
                                index.ContentPart == nameof(ExpensePart) &&
                                index.ContentField == nameof(ExpensePart.Amount) &&
                                index.Numeric != null)
                            .ListAsync())
                        .Select(index => decimal.ToDouble(index.Numeric ?? 0m))
                        .Sum(),
                    },
                },
            },
            Options = new
            {
                // These options below are to simplify UI testing.
                // You can find more detailed description here:
                // https://www.chartjs.org/docs/2.9.4/configuration/
                Animation = new
                {
                    Duration = 0,
                },
                Layout = new
                {
                    Padding = 0,
                },
            },
        });
    // NEXT STATION: Go to Views/Balance.cshtml.

    // Generates a view(Views/History.cshtml) with line chart using <chart> tag helper with two lines to display the monthly amount
    // of incomes and expenses.
    // Both of them can be filtered with a tag assigned to it. The tag comes from OC taxonomy.
    // It uses data from Income and Expense content types stored in db as dataset to it.
    // Lombiq.ChartJs.Samples/Sample/History
    public async Task<IActionResult> History(string incomeTag = null, string expenseTag = null)
    {
        var incomeTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.IncomeTagsTaxonomy, incomeTag);
        var expenseTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.ExpenseTagsTaxonomy, expenseTag);

        var transactions = (await _session.QueryIndex<NumericFieldIndex>(
            index =>
                index.Published &&
                index.Latest &&
                (index.ContentType == ContentTypes.Income || index.ContentType == ContentTypes.Expense) &&
                (index.ContentPart == nameof(IncomePart) || index.ContentPart == nameof(ExpensePart)) &&
                index.ContentField == nameof(TransactionPart.Amount) &&
                index.Numeric != null)
            .ListAsync())
            .Join(
                await _session.QueryIndex<DateFieldIndex>(
                index =>
                    index.Published &&
                    index.Latest &&
                    (index.ContentType == ContentTypes.Income || index.ContentType == ContentTypes.Expense) &&
                    (index.ContentPart == nameof(IncomePart) || index.ContentPart == nameof(ExpensePart)) &&
                    index.ContentField == nameof(TransactionPart.Date) &&
                    index.Date != null)
                .ListAsync(),
                index => index.ContentItemId,
                index => index.ContentItemId,
                (numericIndex, dateIndex) =>
                    new
                    {
                        numericIndex.ContentItemId,
                        numericIndex.ContentType,
                        Date = dateIndex.Date!.Value,
                        Amount = numericIndex.Numeric ?? 0,
                    })
            .Where(
                transaction =>
                    (transaction.ContentType == ContentTypes.Income &&
                        (incomeTagsFilter == null || incomeTagsFilter.Contains(transaction.ContentItemId))) ||
                    (transaction.ContentType == ContentTypes.Expense &&
                        (expenseTagsFilter == null || expenseTagsFilter.Contains(transaction.ContentItemId))))
            .GroupBy(transaction => new DateTime(transaction.Date.Year, transaction.Date.Month, 1))
            .OrderBy(monthly => monthly.Key)
            .Select(monthly =>
                new
                {
                    Date = monthly.Key,
                    Income = monthly
                        .Where(transaction => transaction.ContentType == ContentTypes.Income)
                        .Sum(transaction => decimal.ToDouble(transaction.Amount)) as double?,
                    Expense = monthly
                        .Where(transaction => transaction.ContentType == ContentTypes.Expense)
                        .Sum(transaction => decimal.ToDouble(transaction.Amount)) as double?,
                }
            )
            .ToDictionary(monthly => monthly.Date);

        return View(new HistoryViewModel
        {
            Labels = transactions.Keys
                .OrderBy(item => item)
                .Select(item => item.ToString("MMMM yyyy", CultureInfo.InvariantCulture)),
            // You can find more detailed description about dataset here:
            // https://www.chartjs.org/docs/2.9.4/charts/line.html#dataset-properties
            DataSets = new[]
            {
                new ChartJsDataSet
                {
                    Label = Labels.Incomes,
                    BackgroundColor = new[] { ChartColors.Transparent },
                    BorderColor = new[] { ChartColors.IncomesLineChartBorderColor },
                    Data = transactions
                        .OrderBy(item => item.Key)
                        .Select(item => item.Value.Income),
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { ChartColors.Transparent },
                    BorderColor = new[] { ChartColors.ExpensesLineChartBorderColor },
                    Data = transactions
                        .OrderBy(item => item.Key)
                        .Select(item => item.Value.Expense),
                },
            },
            Options = new
            {
                // These options below are to simplify UI testing.
                // You can find more detailed description here:
                // https://www.chartjs.org/docs/2.9.4/configuration/
                Animation = new
                {
                    Duration = 0,
                },
                Layout = new
                {
                    Padding = 0,
                },
            },
            IncomeTerms = await _contentManager.GetTaxonomyTermsDisplayTextsAsync(ContentItemIds.IncomeTagsTaxonomy),
            ExpenseTerms = await _contentManager.GetTaxonomyTermsDisplayTextsAsync(ContentItemIds.ExpenseTagsTaxonomy),
            IncomeTag = incomeTag,
            ExpenseTag = expenseTag,
        });
        // NEXT STATION: Go to Views/History.cshtml.
    }

    private async Task<IEnumerable<string>> GetItemIdsByTermIdAsync(string taxonomyId, string termId) =>
        string.IsNullOrEmpty(termId)
            ? null
            : (await _orchardHelper.QueryCategorizedContentItemsAsync(query => query
                .Where(taxIndex => taxIndex.TaxonomyContentItemId == taxonomyId)
                .Where(taxIndex => taxIndex.TermContentItemId == termId)))
                .Select(taxIndex => taxIndex.ContentItemId);
}
