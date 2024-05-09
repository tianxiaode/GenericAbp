using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuickTemplate.Data;

/* This is used if database provider does't define
 * IQuickTemplateDbSchemaMigrator implementation.
 */
public class NullQuickTemplateDbSchemaMigrator : IQuickTemplateDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
