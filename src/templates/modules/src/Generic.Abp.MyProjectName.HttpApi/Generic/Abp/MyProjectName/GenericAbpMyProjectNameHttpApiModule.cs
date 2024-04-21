using Localization.Resources.AbpUi;
using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Generic.Abp.MyProjectName
{
    [DependsOn(
        typeof(GenericAbpMyProjectNameApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpMyProjectNameHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMyProjectNameHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyProjectNameResource>();
            });
        }
    }
}
