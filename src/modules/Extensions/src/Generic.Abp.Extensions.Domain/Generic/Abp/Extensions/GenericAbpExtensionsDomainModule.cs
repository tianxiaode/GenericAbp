using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.Extensions;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GenericAbpExtensionsModule)
)]
public class GenericAbpExtensionsDomainModule : AbpModule
{
}