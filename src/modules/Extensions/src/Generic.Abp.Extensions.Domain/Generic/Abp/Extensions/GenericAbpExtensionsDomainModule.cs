using Generic.Abp.Extensions.Trees;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.Extensions;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GenericAbpExtensionsModule)
)]
public class GenericAbpExtensionsDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(ITreeCodeGenerator<>), typeof(TreeCodeGenerator<>));
    }
}