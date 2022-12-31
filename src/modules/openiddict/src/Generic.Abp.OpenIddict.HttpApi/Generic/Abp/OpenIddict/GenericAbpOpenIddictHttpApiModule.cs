using Generic.Abp.OpenIddict.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.OpenIddict
{
    [DependsOn(
        typeof(GenericAbpOpenIddictApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpOpenIddictHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(OpenIddictResource),
                    typeof(GenericAbpOpenIddictApplicationContractsModule).Assembly,
                    typeof(GenericAbpOpenIddictDomainSharedModule).Assembly,
                    typeof(GenericAbpOpenIddictHttpApiModule).Assembly
                );
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpOpenIddictHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OpenIddictResource>();
            });
        }
    }
}
