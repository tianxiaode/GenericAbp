using Volo.Abp.Modularity;

namespace Generic.Abp.MyProjectName;

[DependsOn(
    typeof(GenericAbpMyProjectNameApplicationModule),
    typeof(GenericAbpMyProjectNameDomainTestModule)
    )]
public class GenericAbpMyProjectNameApplicationTestModule : AbpModule
{

}
