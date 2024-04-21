using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.MyProjectName
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(GenericAbpMyProjectNameDomainSharedModule)
    )]
    public class GenericAbpMyProjectNameDomainModule : AbpModule
    {

    }
}
