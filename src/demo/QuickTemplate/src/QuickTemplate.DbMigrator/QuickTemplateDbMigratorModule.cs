using QuickTemplate.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace QuickTemplate.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(QuickTemplateEntityFrameworkCoreModule),
    typeof(QuickTemplateApplicationContractsModule)
    )]
public class QuickTemplateDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
