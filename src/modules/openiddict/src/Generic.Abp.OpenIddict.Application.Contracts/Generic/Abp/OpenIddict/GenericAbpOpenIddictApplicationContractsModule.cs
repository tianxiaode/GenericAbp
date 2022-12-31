using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.OpenIddict
{
    [DependsOn(
        typeof(GenericAbpOpenIddictDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpOpenIddictApplicationContractsModule : AbpModule
    {

    }
}
