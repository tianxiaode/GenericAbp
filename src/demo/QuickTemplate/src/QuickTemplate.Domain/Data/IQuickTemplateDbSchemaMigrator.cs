using System.Threading.Tasks;

namespace QuickTemplate.Data;

public interface IQuickTemplateDbSchemaMigrator
{
    Task MigrateAsync();
}
