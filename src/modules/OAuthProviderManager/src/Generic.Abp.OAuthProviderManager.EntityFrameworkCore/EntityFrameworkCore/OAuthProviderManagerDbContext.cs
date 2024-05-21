using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.OAuthProviderManager.EntityFrameworkCore
{
    [ConnectionStringName(OAuthProviderManagerDbProperties.ConnectionStringName)]
    public class OAuthProviderManagerDbContext : AbpDbContext<OAuthProviderManagerDbContext>, IOAuthProviderManagerDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public OAuthProviderManagerDbContext(DbContextOptions<OAuthProviderManagerDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOAuthProviderManager();
        }
    }
}