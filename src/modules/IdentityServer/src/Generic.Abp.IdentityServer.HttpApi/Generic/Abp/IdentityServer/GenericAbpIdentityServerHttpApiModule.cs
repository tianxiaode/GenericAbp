using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.IdentityServer
{
    [DependsOn(
        typeof(GenericAbpIdentityServerApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpIdentityServerHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpIdentityServerResource),
                    typeof(GenericAbpIdentityServerApplicationContractsModule).Assembly,
                    typeof(GenericAbpIdentityServerDomainSharedModule).Assembly,
                    typeof(GenericAbpIdentityServerHttpApiModule).Assembly
                );
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpIdentityServerHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpIdentityServerResource>();
            });
        }
    }
}
