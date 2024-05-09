using Volo.Abp.Modularity;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateApplicationModule),
    typeof(QuickTemplateDomainTestModule)
    )]
public class QuickTemplateApplicationTestModule : AbpModule
{

}
