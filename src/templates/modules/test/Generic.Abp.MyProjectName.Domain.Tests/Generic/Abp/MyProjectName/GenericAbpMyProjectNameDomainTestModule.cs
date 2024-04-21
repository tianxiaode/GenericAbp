using Volo.Abp.Modularity;

namespace Generic.Abp.MyProjectName;

[DependsOn(
    typeof(GenericAbpMyProjectNameDomainModule),
    typeof(GenericAbpMyProjectNameTestBaseModule)
)]
public class GenericAbpMyProjectNameDomainTestModule : AbpModule
{

}
