using Generic.Abp.Extensions;
using Generic.Abp.Extensions.Localization;
using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpExtensionsModule),
        typeof(AbpValidationModule)
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
                    .AddBaseTypes(typeof(ExtensionsResource))
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Generic/Abp/MenuManagement/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.MenuManagement", typeof(MenuManagementResource));
            });
        }
    }
}