using Generic.Abp.ExtMenu.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.ExtMenu
{
    [DependsOn(
        typeof(GenericAbpExtMenuApplicationModule), 
        typeof(AbpAspNetCoreMvcModule)
        )]

    public class GenericAbpExtMenuHttpApiModule :AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpExtMenuHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ExtMenuResource>();
                // .AddBaseTypes(
                //     typeof(AbpUiResource)
                // );
            });
        }

    }
}
