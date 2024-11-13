using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.PermissionManagement;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpMenuManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
    )]
    public class GenericAbpMenuManagementApplicationContractsModule : AbpModule
    {
    }
}