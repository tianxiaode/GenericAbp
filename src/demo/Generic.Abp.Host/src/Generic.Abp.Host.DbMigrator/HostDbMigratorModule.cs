using Generic.Abp.Host.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Generic.Abp.Host.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HostEntityFrameworkCoreModule),
    typeof(HostApplicationContractsModule)
)]
public class HostDbMigratorModule : AbpModule
{
}
