using Generic.Abp.Domain.Trees;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.Domain;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(GenericAbpDddDomainSharedModule))]
public class GenericAbpDddDomainModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(ITreeCodeGenerator<>), typeof(TreeCodeGenerator<>));
    }

}