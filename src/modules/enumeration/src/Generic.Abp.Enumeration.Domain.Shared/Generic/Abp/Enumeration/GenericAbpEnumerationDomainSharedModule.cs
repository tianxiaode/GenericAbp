using Generic.Abp.Enumeration.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.Enumeration
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class GenericAbpEnumerationDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<EnumerationResource>("en");
            });

            Configure<EnumerationOptions>(options =>
            {
                options
                    .Resources
                    .Add(typeof(DefaultEnumeration));

            });
        }

    }
}
