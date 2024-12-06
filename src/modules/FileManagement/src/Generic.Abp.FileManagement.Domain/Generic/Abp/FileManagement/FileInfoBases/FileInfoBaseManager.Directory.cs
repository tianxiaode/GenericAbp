using System.IO;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Settings;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    protected virtual async Task<string> GetAndCheckTempPathAsync(string hash, string? subPath = null)
    {
        var tempDir = await GetTempPathAsync(hash);
        if (!string.IsNullOrEmpty(subPath))
        {
            tempDir = Path.Combine(tempDir, subPath);
        }

        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }

        return tempDir;
    }

    protected virtual async Task<string> GetTempPathAsync(string hash)
    {
        var storagePath = await GetStoragePathAsync();
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath, "temp", hash);
    }

    public virtual Task<string> GetAccessPathAsync(string hash)
    {
        return Task.FromResult($"{hash[..2]}/{hash.Substring(2, 2)}/{hash.Substring(4, 2)}");
    }

    public virtual async Task<string> GetPhysicalPathAsync(string path, bool isCreated = false)
    {
        var storagePath = await GetStoragePathAsync();
        var dir = Path.Combine(Directory.GetCurrentDirectory(), storagePath, path);
        if (isCreated && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return dir;
    }

    public virtual Task<string> GetFileNameAsync(string hash, string extension)
    {
        return Task.FromResult($"{hash}.{extension}");
    }

    protected virtual Task<string> GetThumbnailFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_thumbnail.png");
    }

    public virtual async Task<string> GetStoragePathAsync()
    {
        return await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.StoragePath);
    }
}