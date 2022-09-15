using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.ExtResource
{
    [DependsOn(
        typeof(GenericAbpExtResourceApplicationModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class GenericAbpExtResourceHttpApiModule: AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {


            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpExtResourceHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<Localization.ExtResource>();
            });
        }

    }
}