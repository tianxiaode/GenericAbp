using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Generic.Abp.AuditLogging
{
    [DependsOn(
        typeof(GenericAbpAuditLoggingDomainModule),
        typeof(GenericAbpAuditLoggingApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class GenericAbpAuditLoggingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpAuditLoggingApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpAuditLoggingApplicationModuleAutoMapperProfile>(validate: true);
            });
        }
    }
}