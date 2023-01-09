using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.PhoneLogin.EntityFrameworkCore
{
    [ConnectionStringName(PhoneLoginDbProperties.ConnectionStringName)]
    public class PhoneLoginDbContext : AbpDbContext<PhoneLoginDbContext>, IPhoneLoginDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public PhoneLoginDbContext(DbContextOptions<PhoneLoginDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePhoneLogin();
        }
    }
}