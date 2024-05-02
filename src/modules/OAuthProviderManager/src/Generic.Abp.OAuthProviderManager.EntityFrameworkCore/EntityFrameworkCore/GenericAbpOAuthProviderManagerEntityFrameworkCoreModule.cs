using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.OAuthProviderManager.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpOAuthProviderManagerDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpOAuthProviderManagerEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OAuthProviderManagerDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}