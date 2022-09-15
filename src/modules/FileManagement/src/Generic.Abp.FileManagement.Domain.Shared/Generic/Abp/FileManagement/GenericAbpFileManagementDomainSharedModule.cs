using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Localization;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.Helper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpHelperFileModule),
        typeof(GenericAbpHelperCommonModule)
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
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddVirtualJson("/Generic/Abp/FileManagement/Localization/FileManagement");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.FileManagement", typeof(FileManagementResource));
            });
        }
    }
}
