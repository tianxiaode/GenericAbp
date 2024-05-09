using QuickTemplate.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace QuickTemplate.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(QuickTemplateEntityFrameworkCoreModule),
    typeof(QuickTemplateApplicationContractsModule)
    )]
public class QuickTemplateDbMigratorModule : AbpModule
{
}
