using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Helpers;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Samples.Migrations;
public class ExpenseMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;

    public ExpenseMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
    }

    public Task<int> CreateAsync() =>
        TransactionMigrationHelpers.CreateTransactionAsync<ExpensePart>(
            _contentDefinitionManager,
            _contentManager,
            SchemaBuilder,
            ContentTypes.Expense,
            ContentItemIds.ExpenseTagsTaxonomy);
}
