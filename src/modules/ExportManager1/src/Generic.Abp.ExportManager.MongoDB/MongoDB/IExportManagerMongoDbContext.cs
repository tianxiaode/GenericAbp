using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.ExportManager.MongoDB;

[ConnectionStringName(ExportManagerDbProperties.ConnectionStringName)]
public interface IExportManagerMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
