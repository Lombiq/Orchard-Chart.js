using Lombiq.ChartJs.Constants;
using Lombiq.ChartJs.Models;
using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.ViewModels.Sample;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

    public async Task<ActionResult> Balance() =>
        View(new BalanceViewModel
        {
            Labels = new[] { Labels.Balance },
            DataSets = new[]
            {
                new ChartJsDataSet
                {
                    Label = Labels.Incomes,
                    BackgroundColor = new[] { "#00aa00" },
                    Data = new[]
                    {
                        (await _session.QueryIndex<IncomePartIndex>()
                            .ListAsync())
                            .Where(income => income.Amount is not null)
                            .Select(income => decimal.ToDouble(income.Amount.Value))
                            .Sum() as double?,
                    },
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { "#aa0000" },
                    Data = new[]
                    {
                        (await _session.QueryIndex<ExpensePartIndex>()
                            .ListAsync())
                            .Where(expense => expense.Amount is not null)
                            .Select(expense => decimal.ToDouble(expense.Amount.Value))
                            .Sum() as double?,
                    },
                },
            },
            Options = new
            {
                Animation = new
                {
                    Duration = 0,
                },
            },
        });

    private async Task<string[]> GetItemIdsByTermIdAsync(string taxonomyId, string termId)
    {
        if (string.IsNullOrEmpty(termId))
        {
            return null;
        }

        return (await _orchardHelper.QueryCategorizedContentItemsAsync(query => query
                .Where(taxIndex => taxIndex.TaxonomyContentItemId == taxonomyId)
                .Where(taxIndex => taxIndex.TermContentItemId == termId)))
                .Select(taxIndex => taxIndex.ContentItemId)
                .ToArray();
    }

    private async Task<IEnumerable<ContentItem>> GetTaxTermItemsAsync(string taxonomyId)
    {
        var taxonomy = await _contentManager.GetAsync(taxonomyId);

        return (taxonomy.Content.TaxonomyPart.Terms as JArray)
            .Select(term => term.ToObject<ContentItem>());
    }

    public async Task<ActionResult> History(string incomeTag = null, string expenseTag = null)
    {
        string[] incomeTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.IncomeTagsTaxonomy, incomeTag);
        string[] expenseTagsFilter = await GetItemIdsByTermIdAsync(ContentItemIds.ExpenseTagsTaxonomy, expenseTag);

        var incomes = (await _session.QueryIndex<IncomePartIndex>()
            .OrderBy(income => income.Date)
            .ListAsync())
            .Where(income => incomeTagsFilter == null || incomeTagsFilter.Contains(income.ContentItemId));

        var expenses = (await _session.QueryIndex<ExpensePartIndex>()
            .OrderBy(expense => expense.Date)
            .ListAsync())
            .Where(income => expenseTagsFilter == null || expenseTagsFilter.Contains(income.ContentItemId));

        var items = incomes.Select(item => item as ItemPartIndex)
            .Concat(expenses.Select(item => item as ItemPartIndex))
            .Where(item => item.Amount is not null)
            .Where(item => item.Date is not null)
            .GroupBy(item => new DateTime(item.Date.Value.Year, item.Date.Value.Month, 1))
            .Select(group =>
                new
                {
                    Date = group.Key,
                    Income = group.OfType<IncomePartIndex>()
                        .Sum(item => decimal.ToDouble(item.Amount.Value)) as double?,
                    Expense = group.OfType<ExpensePartIndex>()
                        .Sum(item => decimal.ToDouble(item.Amount.Value)) as double?,
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
                    BackgroundColor = new[] { "#00000000" },
                    BorderColor = new[] { "#00aa00" },
                    Data = items.OrderBy(item => item.Key)
                        .Select(item => item.Value.Income),
                },
                new ChartJsDataSet
                {
                    Label = Labels.Expenses,
                    BackgroundColor = new[] { "#00000000" },
                    BorderColor = new[] { "#aa0000" },
                    Data = items.OrderBy(item => item.Key)
                        .Select(item => item.Value.Expense),
                },
            },
            Options = new
            {
                Animation = new
                {
                    Duration = 0,
                },
            },
            IncomeTerms = await GetTaxTermItemsAsync(ContentItemIds.IncomeTagsTaxonomy),
            ExpenseTerms = await GetTaxTermItemsAsync(ContentItemIds.ExpenseTagsTaxonomy),
            IncomeTag = incomeTag,
            ExpenseTag = expenseTag,
        });
    }
}
