using Volo.Abp.Modularity;

namespace Generic.Abp.Enumeration
{
    [DependsOn(
        typeof(GenericAbpEnumerationApplicationContractsModule)
    )]

    public class GenericAbpEnumerationApplicationModule : AbpModule
    {
    }
}