using Volo.Abp.Application;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.AuditLogging
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationAbstractionsModule)
    )]
    public class GenericAbpAuditLoggingApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpAuditLoggingApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AuditLoggingResource>();
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.AuditLogging", typeof(AuditLoggingResource));
            });
        }
    }
}