using Volo.Abp.Modularity;

namespace Generic.Abp.OAuthProviderManager;

[DependsOn(
    typeof(GenericAbpOAuthProviderManagerApplicationModule),
    typeof(GenericAbpOAuthProviderManagerDomainTestModule)
    )]
public class GenericAbpOAuthProviderManagerApplicationTestModule : AbpModule
{

}
