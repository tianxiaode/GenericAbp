using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Generic.Abp.Enumeration
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(GenericAbpEnumerationApplicationModule))]

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
