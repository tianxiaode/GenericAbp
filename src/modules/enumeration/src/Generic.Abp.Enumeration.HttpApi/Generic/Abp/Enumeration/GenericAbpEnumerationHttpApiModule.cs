using Generic.Abp.Enumeration.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Generic.Abp.Enumeration
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(GenericAbpEnumerationApplicationContractsModule))]

    public class GenericAbpEnumerationHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpEnumerationHttpApiModule).Assembly);
            });

        }

    }
}
