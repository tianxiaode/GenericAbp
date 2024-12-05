using Generic.Abp.Extensions.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.FileInfoBases;

public interface IFileInfoBaseRepository : IExtensionRepository<FileInfoBase>
{
    Task BulkUpdateExpireAtAsync(FileRetentionPolicy policy, int retentionPeriod, int batchSize,
        CancellationToken cancellationToken = default);

    Task<List<FileInfoBase>> GetCleanupExpiredFilesAsync(FileRetentionPolicy policy, int batchSize,
        CancellationToken cancellationToken = default);
}