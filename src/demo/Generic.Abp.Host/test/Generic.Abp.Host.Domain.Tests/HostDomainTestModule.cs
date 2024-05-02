using Volo.Abp.Modularity;

namespace Generic.Abp.Host;

[DependsOn(
    typeof(HostDomainModule),
    typeof(HostTestBaseModule)
)]
public class HostDomainTestModule : AbpModule
{

}
