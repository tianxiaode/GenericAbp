using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.PhoneLogin.EntityFrameworkCore
{
    [ConnectionStringName(PhoneLoginDbProperties.ConnectionStringName)]
    public interface IPhoneLoginDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}