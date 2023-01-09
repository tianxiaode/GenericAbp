using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.PhoneLogin.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpPhoneLoginDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpPhoneLoginEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PhoneLoginDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}