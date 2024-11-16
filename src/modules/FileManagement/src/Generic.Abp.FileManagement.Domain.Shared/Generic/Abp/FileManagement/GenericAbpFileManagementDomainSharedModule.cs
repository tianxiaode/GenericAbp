using Generic.Abp.Extensions;
using Generic.Abp.Extensions.Localization;
using Generic.Abp.FileManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(GenericAbpExtensionsModule),
        typeof(AbpValidationModule)
    )]
    public class GenericAbpFileManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpFileManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<FileManagementResource>("en")
                    .AddBaseTypes(typeof(ExtensionsResource))
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Generic/Abp/FileManagement/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.FileManagement", typeof(FileManagementResource));
            });
        }
    }
}