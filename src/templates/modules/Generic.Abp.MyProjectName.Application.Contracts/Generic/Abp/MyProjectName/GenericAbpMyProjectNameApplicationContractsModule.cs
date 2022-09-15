using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.MyProjectName
{
    [DependsOn(
        typeof(GenericAbpMyProjectNameDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpMyProjectNameApplicationContractsModule : AbpModule
    {

    }
}
