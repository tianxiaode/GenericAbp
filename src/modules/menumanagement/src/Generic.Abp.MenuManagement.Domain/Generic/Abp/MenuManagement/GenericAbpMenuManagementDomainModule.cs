using Generic.Abp.Extensions;
using Volo.Abp.Modularity;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpExtensionsDomainModule),
        typeof(GenericAbpMenuManagementDomainSharedModule)
    )]
    public class GenericAbpMenuManagementDomainModule : AbpModule
    {
    }
}