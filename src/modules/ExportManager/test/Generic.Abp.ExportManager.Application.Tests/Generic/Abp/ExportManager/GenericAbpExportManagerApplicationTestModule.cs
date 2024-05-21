using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(GenericAbpExportManagerApplicationModule),
    typeof(GenericAbpExportManagerDomainTestModule)
    )]
public class GenericAbpExportManagerApplicationTestModule : AbpModule
{

}
