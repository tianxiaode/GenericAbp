using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(GenericAbpFileManagementDomainSharedModule)
    )]
    public class GenericAbpFileManagementDomainModule : AbpModule
    {

    }
}
