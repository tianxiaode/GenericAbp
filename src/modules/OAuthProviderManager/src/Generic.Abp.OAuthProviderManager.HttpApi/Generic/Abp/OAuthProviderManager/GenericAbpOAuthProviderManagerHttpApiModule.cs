using Localization.Resources.AbpUi;
using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.AspNetCore;

namespace Generic.Abp.OAuthProviderManager
{
    [DependsOn(
            typeof(GenericAbpOAuthProviderManagerApplicationContractsModule),
            typeof(AbpAspNetCoreMvcModule),
            typeof(AbpIdentityAspNetCoreModule)
        )
    ]
    public class GenericAbpOAuthProviderManagerHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpOAuthProviderManagerHttpApiModule)
                    .Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OAuthProviderManagerResource>();
            });
        }
    }
}