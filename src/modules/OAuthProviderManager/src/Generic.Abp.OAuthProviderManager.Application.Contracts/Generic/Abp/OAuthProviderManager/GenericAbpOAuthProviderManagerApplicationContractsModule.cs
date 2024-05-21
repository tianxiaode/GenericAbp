using Generic.Abp.BusinessException.Localization;
using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.VirtualFileSystem;
using Generic.Abp.BusinessException;
using Volo.Abp.Validation.Localization;

namespace Generic.Abp.OAuthProviderManager
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(GenericAbpBusinessExceptionModule),
        typeof(AbpAuthorizationModule)
    )]
    public class GenericAbpOAuthProviderManagerApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpOAuthProviderManagerApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OAuthProviderManagerResource>("en")
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Generic/Abp/OAuthProviderManager/Localization/OAuthProviderManager");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.OAuthProviderManager", typeof(OAuthProviderManagerResource));
            });
        }
    }
}