using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.EntityFrameworkCore
{
    [ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
    public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentity();
        }
    }
}