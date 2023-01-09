using Localization.Resources.AbpUi;
using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Generic.Abp.PhoneLogin
{
    [DependsOn(
        typeof(GenericAbpPhoneLoginApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpPhoneLoginHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpPhoneLoginHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PhoneLoginResource>();
            });
        }
    }
}
