using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.OAuthProviderManager
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(GenericAbpOAuthProviderManagerDomainSharedModule)
    )]
    public class GenericAbpOAuthProviderManagerDomainModule : AbpModule
    {

    }
}
