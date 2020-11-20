using Generic.Abp.Enumeration;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Generic.Abp.AspNetCore.DataAnnotations
{
    [DependsOn(
        typeof(GenericAbpEnumerationDomainSharedModule),
        typeof(AbpAspNetCoreMvcModule)
    )]

    public class GenericAbpEnumerationAspNetCoreModule :AbpModule
    {
        
    }
}