using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Host.Data;

/* This is used if database provider does't define
 * IHostDbSchemaMigrator implementation.
 */
public class NullHostDbSchemaMigrator : IHostDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
