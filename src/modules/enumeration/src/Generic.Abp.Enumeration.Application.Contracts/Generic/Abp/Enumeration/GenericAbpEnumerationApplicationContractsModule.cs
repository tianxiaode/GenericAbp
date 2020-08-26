using Volo.Abp.Modularity;

namespace Generic.Abp.Enumeration
{
    [DependsOn(
        typeof(GenericAbpEnumerationDomainSharedModule)
    )]

    public class GenericAbpEnumerationApplicationContractsModule : AbpModule
    {
    }
}
