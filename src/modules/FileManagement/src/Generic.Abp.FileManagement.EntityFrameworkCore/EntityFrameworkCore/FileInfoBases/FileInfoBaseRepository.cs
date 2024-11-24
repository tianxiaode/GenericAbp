using Generic.Abp.FileManagement.FileInfoBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.FileInfoBases;

public class FileInfoBaseRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : EfCoreRepository<FileManagementDbContext, FileInfoBase, Guid>(dbContextProvider), IFileInfoBaseRepository
{
    public virtual async Task BulkUpdateExpireAtAsync(FileRetentionPolicy policy, int retentionPeriod, int batchSize,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        while (true)
        {
            // 查询需要更新的记录，分批执行
            var fileInfoToUpdate = await dbContext.Set<FileInfoBase>()
                .Where(file => file.ExpireAt == null &&
                               file.RetentionPolicy == policy &&
                               !dbContext.Set<Resource>().Any(resource => resource.FileInfoBaseId == file.Id))
                .Take(batchSize)
                .ToListAsync(cancellationToken);

            if (!fileInfoToUpdate.Any())
            {
                break; // 没有需要更新的记录，退出循环
            }

            // 更新过期时间
            foreach (var file in fileInfoToUpdate)
            {
                file.SetExpireAt(DateTime.UtcNow.AddDays(retentionPeriod));
            }

            // 使用 UpdateManyAsync 提交更新
            await UpdateManyAsync(fileInfoToUpdate, true, cancellationToken);

            if (fileInfoToUpdate.Count < batchSize)
            {
                break; // 已处理所有记录，退出循环
            }
        }
    }

    public virtual async Task<List<FileInfoBase>> GetCleanupExpiredFilesAsync(FileRetentionPolicy policy, int batchSize,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(m => m.ExpireAt <= DateTime.UtcNow && m.RetentionPolicy == policy)
            .Take(batchSize).ToListAsync(cancellationToken);
    }
}