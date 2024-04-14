using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(ExportManagerDomainModule),
    typeof(ExportManagerTestBaseModule)
)]
public class ExportManagerDomainTestModule : AbpModule
{

}
