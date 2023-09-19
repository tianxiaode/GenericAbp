using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Localization;
using Generic.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpDddDomainSharedModule)
    )]
    public class GenericAbpMenuManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpMenuManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MenuManagementResource>("en")
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Generic/Abp/MenuManagement/Localization/MenuManagement");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.MenuManagement", typeof(MenuManagementResource));
            });
        }
    }
}