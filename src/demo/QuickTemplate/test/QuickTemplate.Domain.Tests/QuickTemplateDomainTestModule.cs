using QuickTemplate.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateEntityFrameworkCoreTestModule)
    )]
public class QuickTemplateDomainTestModule : AbpModule
{

}
