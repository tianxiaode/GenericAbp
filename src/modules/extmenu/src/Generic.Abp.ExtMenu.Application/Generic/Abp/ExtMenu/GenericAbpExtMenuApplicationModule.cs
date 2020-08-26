using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Generic.Abp.ExtMenu
{
    [DependsOn(
        typeof(GenericAbpExtMenuApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule)
    )]

    public class GenericAbpExtMenuApplicationModule : AbpModule
    {

    }
}
