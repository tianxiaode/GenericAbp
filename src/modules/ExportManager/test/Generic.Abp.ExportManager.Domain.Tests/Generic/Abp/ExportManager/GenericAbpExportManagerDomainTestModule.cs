using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(GenericAbpExportManagerDomainModule),
    typeof(GenericAbpExportManagerTestBaseModule)
)]
public class GenericAbpExportManagerDomainTestModule : AbpModule
{

}
