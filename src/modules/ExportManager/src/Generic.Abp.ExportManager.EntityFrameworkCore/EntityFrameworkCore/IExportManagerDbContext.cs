using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.ExportManager.EntityFrameworkCore;

[ConnectionStringName(ExportManagerDbProperties.ConnectionStringName)]
public interface IExportManagerDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
