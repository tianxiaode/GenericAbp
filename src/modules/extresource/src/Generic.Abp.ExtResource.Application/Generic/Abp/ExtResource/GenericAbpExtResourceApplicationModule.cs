using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Generic.Abp.ExtResource
{
    [DependsOn(
        typeof(GenericAbpExtResourceApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpDddApplicationModule)
        )]
    public class GenericAbpExtResourceApplicationModule: AbpModule
    {
        
    }
}