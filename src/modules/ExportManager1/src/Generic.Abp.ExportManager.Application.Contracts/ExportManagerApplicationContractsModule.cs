using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(ExportManagerDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ExportManagerApplicationContractsModule : AbpModule
{

}
