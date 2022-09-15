using Generic.Abp.Demo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Generic.Abp.Demo.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(DemoEntityFrameworkCoreDbMigrationsModule),
        typeof(DemoApplicationContractsModule)
        )]
    public class DemoDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
