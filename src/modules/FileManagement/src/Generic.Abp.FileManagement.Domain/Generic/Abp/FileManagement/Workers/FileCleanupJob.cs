using System.Threading.Tasks;
using System.Threading;
using System;
using Generic.Abp.FileManagement.FileInfoBases;
using Volo.Abp.BackgroundWorkers;

namespace Generic.Abp.FileManagement.Workers;

public class FileCleanupJob : IBackgroundWorker
{
    private readonly IFileInfoBaseRepository _fileRepository;

    public FileCleanupJob(IFileInfoBaseRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var filesToDelete = await _fileRepository.GetListAsync(f =>
            f.ExpireAt != null && f.ExpireAt <= now && f.RetentionPolicy != FileRetentionPolicy.Retain);

        foreach (var file in filesToDelete)
        {
            await _fileRepository.DeleteAsync(file.Id);
            // TODO: 删除物理文件
        }
    }
}