using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(GenericAbpIdentityApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpIdentityHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource),
                    typeof(GenericAbpIdentityApplicationContractsModule).Assembly,
                    typeof(GenericAbpIdentityDomainSharedModule).Assembly,
                    typeof(GenericAbpIdentityHttpApiModule).Assembly
                );
            });


            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpIdentityHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(GenericAbpIdentityHttpApiModule).Assembly);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>();
            });
        }
    }
}