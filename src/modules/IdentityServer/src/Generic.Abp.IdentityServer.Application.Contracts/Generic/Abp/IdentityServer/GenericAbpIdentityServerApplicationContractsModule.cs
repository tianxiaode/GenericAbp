using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Generic.Abp.IdentityServer
{
    [DependsOn(
        typeof(GenericAbpIdentityServerDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpIdentityServerApplicationContractsModule : AbpModule
    {

    }
}
