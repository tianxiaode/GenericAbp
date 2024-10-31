using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.AuditLogging
{
    [DependsOn(
        typeof(GenericAbpAuditLoggingApplicationContractsModule),
        typeof(AbpAspNetCoreModule)
    )]
    public class GenericAbpAuditLoggingHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AuditLoggingResource),
                    typeof(GenericAbpAuditLoggingApplicationContractsModule).Assembly,
                    typeof(GenericAbpAuditLoggingHttpApiModule).Assembly
                );
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpAuditLoggingHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(GenericAbpAuditLoggingHttpApiModule).Assembly);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AuditLoggingResource>();
            });
        }
    }
}