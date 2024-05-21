using Volo.Abp.Modularity;

namespace Generic.Abp.OAuthProviderManager;

[DependsOn(
    typeof(GenericAbpOAuthProviderManagerDomainModule),
    typeof(GenericAbpOAuthProviderManagerTestBaseModule)
)]
public class GenericAbpOAuthProviderManagerDomainTestModule : AbpModule
{

}
