using Localization.Resources.AbpUi;
using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpMenuManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpMenuManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMenuManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MenuManagementResource>();
            });
        }
    }
}
