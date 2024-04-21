using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.ExportManager.MongoDB;

[ConnectionStringName(ExportManagerDbProperties.ConnectionStringName)]
public class ExportManagerMongoDbContext : AbpMongoDbContext, IExportManagerMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureExportManager();
    }
}
