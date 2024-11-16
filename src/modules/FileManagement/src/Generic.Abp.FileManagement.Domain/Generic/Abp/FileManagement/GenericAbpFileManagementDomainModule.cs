using Generic.Abp.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(GenericAbpExtensionsDomainModule),
        typeof(GenericAbpFileManagementDomainSharedModule)
    )]
    public class GenericAbpFileManagementDomainModule : AbpModule
    {
    }
}