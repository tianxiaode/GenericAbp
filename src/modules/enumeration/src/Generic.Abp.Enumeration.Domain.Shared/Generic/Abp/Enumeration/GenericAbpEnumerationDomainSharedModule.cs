using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace Generic.Abp.Enumeration
{
    [DependsOn(
            typeof(AbpValidationModule)
        )]
    public class GenericAbpEnumerationDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<EnumerationOptions>(options =>
            {
                options
                    .Resources
                    .Add(typeof(DefaultEnumeration));

            });
        }

    }
}
