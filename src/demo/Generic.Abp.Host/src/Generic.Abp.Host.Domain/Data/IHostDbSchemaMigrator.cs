using System.Threading.Tasks;

namespace Generic.Abp.Host.Data;

public interface IHostDbSchemaMigrator
{
    Task MigrateAsync();
}
