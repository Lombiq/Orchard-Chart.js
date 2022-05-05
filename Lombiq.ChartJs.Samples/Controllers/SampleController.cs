using Lombiq.ChartJs.Constants;
using Lombiq.ChartJs.Models;
using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.ViewModels.Sample;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
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

    // Generates a bar chart using <chart> tag helper with two bars to display the actual balance.
    // It uses data from Income and Expense content types stored in db as dataset to it.
    // Lombiq.ChartJs.Samples/Sample/Balance
    public async Task<ActionResult> Balance() =>
        View(new BalanceViewModel
        {
            Labels = new[] { Labels.Balance },
            DataSets = new[]
            {
                new ChartJsDataSet
                {
                    Label = Labels.Incomes,
                    BackgroundColor = new[] { ChartColors.IncomesBarChartBackgroundColor },
                    Data = new double?[]
                    {
                        (await _session.QueryIndex<IncomePartIndex>()
                            .ListAsync())
                            .Where(income => income.Amount is not null)
                            .Select(income => decimal.ToDouble(income.Amount.Value))
                            .Sum(),
                    },
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { ChartColors.ExpensesBarChartBackgroundColor },
                    Data = new double?[]
                    {
                        (await _session.QueryIndex<ExpensePartIndex>()
                            .ListAsync())
                            .Where(expense => expense.Amount is not null)
                            .Select(expense => decimal.ToDouble(expense.Amount.Value))
                            .Sum(),
                    },
                },
            },
            Options = new
            {
                Animation = new
                {
                    Duration = 0,
                },
                Layout = new
                {
                    AutoPadding = false,
                    Padding = 0,
                    DevicePixelRatio = 1,
                },
            },
        });

    private async Task<IEnumerable<string>> GetItemIdsByTermIdAsync(string taxonomyId, string termId) =>
        string.IsNullOrEmpty(termId)
            ? null
            : (await _orchardHelper.QueryCategorizedContentItemsAsync(query => query
                .Where(taxIndex => taxIndex.TaxonomyContentItemId == taxonomyId)
                .Where(taxIndex => taxIndex.TermContentItemId == termId)))
                .Select(taxIndex => taxIndex.ContentItemId);

    // Generates a line chart using <chart> tag helper with two lines to display the monthly amount
    // of incomes and expenses.
    // Both of them can be filtered with a tag assigned to it. The tag comes from OC taxonomy.
    // It uses data from Income and Expense content types stored in db as dataset to it.
    // Lombiq.ChartJs.Samples/Sample/History
    public async Task<ActionResult> History(string incomeTag = null, string expenseTag = null)
    {
        var incomeTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.IncomeTagsTaxonomy, incomeTag);
        var expenseTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.ExpenseTagsTaxonomy, expenseTag);

        var incomes = (await _session.QueryIndex<IncomePartIndex>()
            .OrderBy(income => income.Date)
            .ListAsync())
            .Where(income => incomeTagsFilter == null || incomeTagsFilter.Contains(income.ContentItemId));

        var expenses = (await _session.QueryIndex<ExpensePartIndex>()
            .OrderBy(expense => expense.Date)
            .ListAsync())
            .Where(income => expenseTagsFilter == null || expenseTagsFilter.Contains(income.ContentItemId));

        var items = incomes
            .OfType<TransactionPartIndex>()
            .Concat(expenses.OfType<TransactionPartIndex>())
            .Where(item => item.Amount is not null && item.Date is not null)
            .GroupBy(item => new DateTime(item.Date.Value.Year, item.Date.Value.Month, 1))
            .Select(group =>
                new
                {
                    Date = group.Key,
                    Income = group
                        .OfType<IncomePartIndex>()
                        .Sum(item => decimal.ToDouble(item.Amount ?? 0)) as double?,
                    Expense = group
                        .OfType<ExpensePartIndex>()
                        .Sum(item => decimal.ToDouble(item.Amount ?? 0)) as double?,
                }
            )
            .ToDictionary(item => item.Date);

        return View(new HistoryViewModel
        {
            Labels = items.Keys
                .OrderBy(item => item)
                .Select(item => item.ToString("MMMM yyyy", CultureInfo.InvariantCulture)),
            DataSets = new[]
            {
                new ChartJsDataSet
                {
                    Label = Labels.Incomes,
                    BackgroundColor = new[] { ChartColors.Transparent },
                    BorderColor = new[] { ChartColors.IncomesLineChartBorderColor },
                    Data = items
                        .OrderBy(item => item.Key)
                        .Select(item => item.Value.Income),
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { ChartColors.Transparent },
                    BorderColor = new[] { ChartColors.ExpensesLineChartBorderColor },
                    Data = items
                        .OrderBy(item => item.Key)
                        .Select(item => item.Value.Expense),
                },
            },
            Options = new
            {
                Animation = new
                {
                    Duration = 0,
                },
                Layout = new
                {
                    AutoPadding = false,
                    Padding = 0,
                    DevicePixelRatio = 1,
                },
            },
            IncomeTerms = await _contentManager.GetTaxonomyTermsDisplayTextsAsync(ContentItemIds.IncomeTagsTaxonomy),
            ExpenseTerms = await _contentManager.GetTaxonomyTermsDisplayTextsAsync(ContentItemIds.ExpenseTagsTaxonomy),
            IncomeTag = incomeTag,
            ExpenseTag = expenseTag,
        });
    }
}
