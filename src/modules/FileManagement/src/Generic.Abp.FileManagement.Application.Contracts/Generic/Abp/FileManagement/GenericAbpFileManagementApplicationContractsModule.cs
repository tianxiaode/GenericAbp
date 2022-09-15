using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(GenericAbpFileManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpFileManagementApplicationContractsModule : AbpModule
    {

    }
}
