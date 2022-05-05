using Lombiq.ChartJs.Samples.Constants;
using Lombiq.ChartJs.Samples.Helpers;
using Lombiq.ChartJs.Samples.Indexes;
using Lombiq.ChartJs.Samples.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;

namespace Lombiq.ChartJs.Samples.Migrations;

public class IncomeMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IContentManager _contentManager;

    public IncomeMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _contentManager = contentManager;
    }

    public Task<int> CreateAsync() =>
        TransactionMigrationHelpers.CreateTransactionAsync<IncomePart, IncomePartIndex>(
            _contentDefinitionManager,
            _contentManager,
            SchemaBuilder,
            ContentTypes.Income,
            ContentItemIds.IncomeTagsTaxonomy);
}
