using Generic.Abp.Extensions;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpCachingModule),
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpExtensionsDomainModule),
        typeof(GenericAbpFileManagementDomainSharedModule)
    )]
    public class GenericAbpFileManagementDomainModule : AbpModule
    {
    }
}