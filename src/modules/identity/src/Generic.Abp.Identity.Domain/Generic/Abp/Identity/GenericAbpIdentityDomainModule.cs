using Generic.Abp.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpExtensionsModule)
    )]
    public class GenericAbpIdentityDomainModule : AbpModule
    {
    }
}