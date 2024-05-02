using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.OAuthProviderManager
{
    [DependsOn(
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpDddDomainSharedModule)
    )]
    public class GenericAbpOAuthProviderManagerDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpOAuthProviderManagerDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OAuthProviderManagerResource>("en")
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddVirtualJson("/Generic/Abp/OAuthProviderManager/Localization/OAuthProviderManager");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.OAuthProviderManager", typeof(OAuthProviderManagerResource));
            });
        }
    }
}
