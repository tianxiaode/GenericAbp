using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.AuditLogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpAuditLoggingDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpAuditLoggingEntityFrameworkCoreModule : AbpModule
    {
    }
}