using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(ExportManagerApplicationModule),
    typeof(ExportManagerDomainTestModule)
    )]
public class ExportManagerApplicationTestModule : AbpModule
{

}
