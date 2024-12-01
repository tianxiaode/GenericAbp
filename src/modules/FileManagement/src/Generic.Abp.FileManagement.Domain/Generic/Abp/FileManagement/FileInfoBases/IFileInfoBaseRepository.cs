using System;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using Generic.Abp.Extensions.Entities;

namespace Generic.Abp.FileManagement.FileInfoBases;

public interface IFileInfoBaseRepository : IExtensionRepository<FileInfoBase>
{
    Task BulkUpdateExpireAtAsync(FileRetentionPolicy policy, int retentionPeriod, int batchSize,
        CancellationToken cancellationToken = default);

    Task<List<FileInfoBase>> GetCleanupExpiredFilesAsync(FileRetentionPolicy policy, int batchSize,
        CancellationToken cancellationToken = default);
}