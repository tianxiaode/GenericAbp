using Generic.Abp.Domain;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(GenericAbpMenuManagementDomainSharedModule)
    )]
    public class GenericAbpMenuManagementDomainModule : AbpModule
    {

    }
}
