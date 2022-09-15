using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Generic.Abp.Demo.Data;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Demo.EntityFrameworkCore
{
    public class EntityFrameworkCoreDemoDbSchemaMigrator
        : IDemoDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreDemoDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the DemoMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<DemoMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}