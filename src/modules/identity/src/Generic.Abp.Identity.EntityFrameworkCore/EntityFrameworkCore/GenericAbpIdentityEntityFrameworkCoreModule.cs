using Generic.Abp.Identity.EntityFrameworkCore.Roles;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Generic.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpIdentityDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<IdentityRole, RoleRepository>();

            });
        }
    }
}