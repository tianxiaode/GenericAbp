using Volo.Abp.Modularity;

namespace Generic.Abp.Host;

[DependsOn(
    typeof(HostApplicationModule),
    typeof(HostDomainTestModule)
)]
public class HostApplicationTestModule : AbpModule
{

}
