using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpIdentityDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpIdentityEntityFrameworkCoreModule : AbpModule
    {
    }
}