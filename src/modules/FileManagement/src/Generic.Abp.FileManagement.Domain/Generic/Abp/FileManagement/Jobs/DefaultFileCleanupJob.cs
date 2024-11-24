using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DistributedLocking;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultFileCleanupJob(
    ISettingManager settingManager,
    IFileInfoBaseRepository repository,
    IAbpDistributedLock distributedLock,
    FileInfoBaseManager fileInfoBaseManager)
    : JobBase<DefaultFileCleanupJobArgs>(settingManager, repository, distributedLock)
{
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    protected override async Task StartAsync(DefaultFileCleanupJobArgs args)
    {
        var fixedBatchSize = args.BatchSize;

        var batchSize = fixedBatchSize; // 动态调整的处理逻辑内部批量
        var hasMoreRecords = true;

        while (hasMoreRecords)
        {
            var filesToDelete =
                await Repository.GetCleanupExpiredFilesAsync(FileRetentionPolicy.Default, fixedBatchSize);

            if (!filesToDelete.Any())
            {
                hasMoreRecords = false;
                break;
            }

            var startTime = DateTime.UtcNow;
            var deleteFailedList = new List<Guid>();

            foreach (var fileInfo in filesToDelete.Take(batchSize)) // 处理内部动态批次
            {
                try
                {
                    await FileInfoBaseManager.DeleteAsync(fileInfo, true);
                    Logger.LogInformation($"Deleted file: {fileInfo.Path}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Failed to delete file: {fileInfo.Path}");
                    deleteFailedList.Add(fileInfo.Id);
                }
            }

            var successfulFiles = filesToDelete
                .Take(batchSize)
                .Where(m => !deleteFailedList.Contains(m.Id))
                .ToList();

            if (successfulFiles.Any())
            {
                await Repository.DeleteManyAsync(successfulFiles);
            }

            var endTime = DateTime.UtcNow;
            var processingTime = (endTime - startTime).TotalMilliseconds;

            batchSize = processingTime switch
            {
                > 1000 => Math.Max(batchSize / 2, 100),
                < 500 => Math.Min(batchSize * 2, fixedBatchSize), // 不能超过固定分页大小
                _ => batchSize
            };

            // 如果本次处理的文件数量小于分页大小，说明没有更多记录
            if (filesToDelete.Count < fixedBatchSize)
            {
                hasMoreRecords = false;
            }
        }
    }
}