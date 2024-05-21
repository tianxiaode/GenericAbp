using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.OAuthProviderManager.EntityFrameworkCore
{
    [ConnectionStringName(OAuthProviderManagerDbProperties.ConnectionStringName)]
    public interface IOAuthProviderManagerDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}