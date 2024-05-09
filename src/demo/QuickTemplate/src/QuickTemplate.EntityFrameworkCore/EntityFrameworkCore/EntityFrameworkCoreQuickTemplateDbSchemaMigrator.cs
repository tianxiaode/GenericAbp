using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickTemplate.Data;
using Volo.Abp.DependencyInjection;

namespace QuickTemplate.EntityFrameworkCore;

public class EntityFrameworkCoreQuickTemplateDbSchemaMigrator
    : IQuickTemplateDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreQuickTemplateDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the QuickTemplateDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<QuickTemplateDbContext>()
            .Database
            .MigrateAsync();
    }
}
