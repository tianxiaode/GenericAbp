using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.ExportManager
{
    [DependsOn(
        typeof(GenericAbpExportManagerDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpExportManagerApplicationContractsModule : AbpModule
    {

    }
}
