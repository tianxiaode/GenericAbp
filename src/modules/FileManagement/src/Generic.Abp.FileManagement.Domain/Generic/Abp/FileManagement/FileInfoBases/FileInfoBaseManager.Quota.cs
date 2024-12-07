using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    public virtual async Task<FolderBalanceOfQuotaCacheItem> GetFolderBalanceOfQuotaCacheItemAsync(Guid folderId,
        long storageQuota, long usedStorage)
    {
        var cacheKey = await GetFolderBalanceOfQuotaCacheKeyAsync(folderId);
        var cacheItem = await FolderBalanceOfQuotaCache.GetOrAddAsync(
            cacheKey,
            () => Task.FromResult(new FolderBalanceOfQuotaCacheItem() { Balance = storageQuota - usedStorage }),
            () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(5)
            },
            token: CancellationToken
        );
        if (cacheItem == null)
        {
            throw new Exception("FolderBalanceOfQuotaCacheItem is null");
        }

        return cacheItem;
    }

    protected virtual async Task AdjustFolderQuotaWithLockAsync(Guid folderId, long adjustment)
    {
        // 使用 IDistributedLock 实现分布式锁
        var lockKey = $"Lock:FileManagement:FolderQuota:{folderId}";
        await using var distributedLock = await DistributedLock.TryAcquireAsync(lockKey, TimeSpan.FromSeconds(10));
        if (distributedLock == null)
        {
            throw new Exception($"Failed to acquire lock for folder {folderId}. LockKey: {lockKey}");
        }

        // 获取当前余额
        var cacheItem = await GetFolderBalanceOfQuotaCacheItemAsync(folderId, 0, 0);
        var newBalance = cacheItem.Balance - adjustment;

        // 更新余额
        await UpdateFolderBalanceOfQuotaCacheItemAsync(folderId, newBalance);
    }

    protected virtual async Task UpdateFolderBalanceOfQuotaCacheItemAsync(Guid folderId, long balance)
    {
        var cacheKey = await GetFolderBalanceOfQuotaCacheKeyAsync(folderId);
        Logger.LogDebug("Updating folder quota balance. FolderId: {folderId}, NewBalance: {balance}", folderId,
            balance);

        await FolderBalanceOfQuotaCache.SetAsync(cacheKey, new FolderBalanceOfQuotaCacheItem() { Balance = balance });
    }


    protected virtual Task<string> GetFolderBalanceOfQuotaCacheKeyAsync(Guid folderId)
    {
        return Task.FromResult($"FileManagement_FileInfoBaseManager_{folderId}_FolderBalanceOfQuota");
    }
}