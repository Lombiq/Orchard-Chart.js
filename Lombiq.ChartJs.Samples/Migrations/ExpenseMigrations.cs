using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Helpers;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Samples.Migrations;
public class ExpenseMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public Task<int> CreateAsync() =>
        TransactionMigrationHelpers.CreateTransactionAsync<ExpensePart>(
            contentDefinitionManager,
            contentManager,
            SchemaBuilder,
            ContentTypes.Expense,
            ContentItemIds.ExpenseTagsTaxonomy);
}
