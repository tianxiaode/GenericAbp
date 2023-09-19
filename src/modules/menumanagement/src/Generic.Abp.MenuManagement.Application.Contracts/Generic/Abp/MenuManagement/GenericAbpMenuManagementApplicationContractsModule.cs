using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpMenuManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpMenuManagementApplicationContractsModule : AbpModule
    {

    }
}
