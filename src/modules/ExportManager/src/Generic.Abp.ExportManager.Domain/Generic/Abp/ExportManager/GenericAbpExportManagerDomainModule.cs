using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(GenericAbpExportManagerDomainSharedModule)
    )]
    public class GenericAbpExportManagerDomainModule : AbpModule
    {

    }
}
