using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Helpers;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Samples.Migrations;

public class IncomeMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public Task<int> CreateAsync() =>
        TransactionMigrationHelpers.CreateTransactionAsync<IncomePart>(
            contentDefinitionManager,
            contentManager,
            SchemaBuilder,
            ContentTypes.Income,
            ContentItemIds.IncomeTagsTaxonomy);
}
