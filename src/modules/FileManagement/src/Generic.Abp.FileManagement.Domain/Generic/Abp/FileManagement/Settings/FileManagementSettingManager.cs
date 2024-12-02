using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings.Result;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.Settings;

public class FileManagementSettingManager(ISettingManager settingManager, IDistributedEventBus distributedEventBus)
    : DomainService
{
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;

    public virtual async Task<SettingResult> GetAsync()
    {
        var result = new SettingResult()
        {
            StoragePath = await GetStoragePathAsync(),
            FolderCopyMaxNodeCount = await GetFolderCopyMaxNodeCountAsync(),
            ExpirationDateOfExternalShared = await GetExpirationDateOfExternalSharedAsync(),
            PublicFolder = await GetPublicFolderSettingAsync(),
            SharedFolder = await GetSharedFolderSettingAsync(),
            UsersFolder = await GetUserFolderSettingAsync(),
            DefaultFile = await GetDefaultFileSettingAsync(),
            TemporaryFile = await GetTemporaryFileSettingAsync()
        };
        return result;
    }

    public virtual async Task UpdateAsync(SettingResult settingResult)
    {
        var oldDefaultFileUpdateEnable = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.Enable, false);
        var oldDefaultFileCleanupEnable =
            await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Enable, false);
        var oldTemporaryFileUpdateEnable =
            await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.Enable, false);
        var oldTemporaryFileCleanupEnable =
            await GetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.Enable, false);
        await SetSettingAsync(FileManagementSettings.StoragePath, settingResult.StoragePath);
        await SetSettingAsync(FileManagementSettings.FolderCopyMaxNodeCount, settingResult.FolderCopyMaxNodeCount);
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultQuota,
            settingResult.PublicFolder.StorageQuota);
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileMaxSize,
            settingResult.PublicFolder.MaxFileSize);
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileTypes,
            settingResult.PublicFolder.AllowFileTypes);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultQuota,
            settingResult.SharedFolder.StorageQuota);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileMaxSize,
            settingResult.SharedFolder.MaxFileSize);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileTypes,
            settingResult.SharedFolder.AllowFileTypes);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultQuota, settingResult.UsersFolder.StorageQuota);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultFileMaxSize,
            settingResult.UsersFolder.MaxFileSize);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultFileTypes,
            settingResult.UsersFolder.AllowFileTypes);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultQuota, settingResult.VirtualPath.StorageQuota);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileMaxSize,
            settingResult.VirtualPath.MaxFileSize);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileTypes,
            settingResult.VirtualPath.AllowFileTypes);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Update.Enable,
            settingResult.DefaultFile.UpdateSetting.Enable);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Update.RetentionPeriod,
            settingResult.DefaultFile.UpdateSetting.RetentionPeriod);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Update.Frequency,
            settingResult.DefaultFile.UpdateSetting.Frequency);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Update.BatchSize,
            settingResult.DefaultFile.UpdateSetting.BatchSize);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Enable,
            settingResult.DefaultFile.CleanupSetting.Enable);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Frequency,
            settingResult.DefaultFile.CleanupSetting.Frequency);
        await SetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.BatchSize,
            settingResult.DefaultFile.CleanupSetting.BatchSize);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Update.Enable,
            settingResult.TemporaryFile.UpdateSetting.Enable);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Update.RetentionPeriod,
            settingResult.TemporaryFile.UpdateSetting.RetentionPeriod);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Update.Frequency,
            settingResult.TemporaryFile.UpdateSetting.Frequency);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Update.BatchSize,
            settingResult.TemporaryFile.UpdateSetting.BatchSize);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.Enable,
            settingResult.TemporaryFile.CleanupSetting.Enable);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.Frequency,
            settingResult.TemporaryFile.CleanupSetting.Frequency);
        await SetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.BatchSize,
            settingResult.TemporaryFile.CleanupSetting.BatchSize);

        await DistributedEventBus.PublishAsync(new FileCleanupSettingChangeEto()
        {
            DefaultFileUpdateEnabledChanged =
                oldDefaultFileUpdateEnable == settingResult.DefaultFile.UpdateSetting.Enable,
            DefaultFileCleanupEnabledChanged =
                oldDefaultFileCleanupEnable == settingResult.DefaultFile.CleanupSetting.Enable,
            TemporaryFileUpdateEnabledChanged =
                oldTemporaryFileUpdateEnable == settingResult.TemporaryFile.UpdateSetting.Enable,
            TemporaryFileCleanupEnabledChanged =
                oldTemporaryFileCleanupEnable == settingResult.TemporaryFile.CleanupSetting.Enable,
        });
    }

    public virtual async Task<string> GetStoragePathAsync()
    {
        return await GetSettingAsync(FileManagementSettings.StoragePath,
            FileManagementDefaultSettings.DefaultStoragePath);
    }

    public virtual async Task<int> GetFolderCopyMaxNodeCountAsync()
    {
        return await GetSettingAsync(FileManagementSettings.FolderCopyMaxNodeCount,
            FileManagementDefaultSettings.FolderCopyMaxNodeCount);
    }

    public virtual async Task<int> GetExpirationDateOfExternalSharedAsync()
    {
        return await GetSettingAsync(FileManagementSettings.ExpirationDateOfExternalShared,
            FileManagementDefaultSettings.ExpirationDateOfExternalShared);
    }

    public virtual async Task<FolderSetting> GetPublicFolderSettingAsync()
    {
        return new FolderSetting()
        {
            StorageQuota = (await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultQuota,
                FileManagementDefaultSettings.PublicFolder.DefaultQuota)).ParseToBytes(),
            MaxFileSize = (await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileMaxSize,
                FileManagementDefaultSettings.PublicFolder.DefaultFileMaxSize)).ParseToBytes(),
            AllowFileTypes = await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileTypes,
                FileManagementDefaultSettings.PublicFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetSharedFolderSettingAsync()
    {
        return new FolderSetting()
        {
            StorageQuota = (await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultQuota,
                FileManagementDefaultSettings.SharedFolder.DefaultQuota)).ParseToBytes(),
            MaxFileSize = (await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileMaxSize,
                FileManagementDefaultSettings.SharedFolder.DefaultFileMaxSize)).ParseToBytes(),
            AllowFileTypes = await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileTypes,
                FileManagementDefaultSettings.SharedFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetUserFolderSettingAsync()
    {
        return new FolderSetting()
        {
            StorageQuota = (await GetSettingAsync(FileManagementSettings.UserFolder.DefaultQuota,
                FileManagementDefaultSettings.UserFolder.DefaultQuota)).ParseToBytes(),
            MaxFileSize = (await GetSettingAsync(FileManagementSettings.UserFolder.DefaultFileMaxSize,
                FileManagementDefaultSettings.UserFolder.DefaultFileMaxSize)).ParseToBytes(),
            AllowFileTypes = await GetSettingAsync(FileManagementSettings.UserFolder.DefaultFileTypes,
                FileManagementDefaultSettings.UserFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetParticipantIsolationFolderSettingAsync()
    {
        return new FolderSetting()
        {
            StorageQuota = (await GetSettingAsync(FileManagementSettings.ParticipantIsolationFolder.DefaultQuota,
                FileManagementDefaultSettings.ParticipantIsolationFolder.DefaultQuota)).ParseToBytes(),
            MaxFileSize = (await GetSettingAsync(
                FileManagementSettings.ParticipantIsolationFolder.DefaultFileMaxSize,
                FileManagementDefaultSettings.ParticipantIsolationFolder.DefaultFileMaxSize)).ParseToBytes(),
            AllowFileTypes = await GetSettingAsync(FileManagementSettings.ParticipantIsolationFolder.DefaultFileTypes,
                FileManagementDefaultSettings.ParticipantIsolationFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FileSetting> GetDefaultFileSettingAsync()
    {
        return new FileSetting()
        {
            UpdateSetting = await GetDefaultFileUpdateSettingAsync(),
            CleanupSetting = await GetDefaultFileCleanupSettingAsync()
        };
    }

    public virtual async Task<FileSetting> GetTemporaryFileSettingAsync()
    {
        return new FileSetting()
        {
            UpdateSetting = await GetTemporaryFileUpdateSettingAsync(),
            CleanupSetting = await GetTemporaryFileCleanupSettingAsync()
        };
    }

    public virtual async Task<FileUpdateSetting> GetDefaultFileUpdateSettingAsync()
    {
        return new FileUpdateSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.Enable,
                FileManagementDefaultSettings.DefaultFile.Update.Enable),
            RetentionPeriod = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.RetentionPeriod,
                FileManagementDefaultSettings.DefaultFile.Update.RetentionPeriod),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.Frequency,
                FileManagementDefaultSettings.DefaultFile.Update.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.BatchSize,
                FileManagementDefaultSettings.DefaultFile.Update.BatchSize),
        };
    }

    public virtual async Task<FileCleanupSetting> GetDefaultFileCleanupSettingAsync()
    {
        return new FileCleanupSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Enable,
                FileManagementDefaultSettings.DefaultFile.Cleanup.Enable),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Frequency,
                FileManagementDefaultSettings.DefaultFile.Cleanup.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.BatchSize,
                FileManagementDefaultSettings.DefaultFile.Cleanup.BatchSize),
        };
    }

    public virtual async Task<FileUpdateSetting> GetTemporaryFileUpdateSettingAsync()
    {
        return new FileUpdateSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.Enable,
                FileManagementDefaultSettings.TemporaryFile.Update.Enable),
            RetentionPeriod = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.RetentionPeriod,
                FileManagementDefaultSettings.TemporaryFile.Update.RetentionPeriod),
            Frequency = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.Frequency,
                FileManagementDefaultSettings.TemporaryFile.Update.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.BatchSize,
                FileManagementDefaultSettings.TemporaryFile.Update.BatchSize),
        };
    }

    public virtual async Task<FileCleanupSetting> GetTemporaryFileCleanupSettingAsync()
    {
        return new FileCleanupSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.Enable,
                FileManagementDefaultSettings.TemporaryFile.Cleanup.Enable),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Frequency,
                FileManagementDefaultSettings.TemporaryFile.Cleanup.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.BatchSize,
                FileManagementDefaultSettings.TemporaryFile.Cleanup.BatchSize),
        };
    }

    public virtual async Task<T> GetSettingAsync<T>(string name, T defaultValue)
    {
        var value = await SettingManager.GetOrNullForCurrentTenantAsync(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        try
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(value);
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(value);
            }

            if (typeof(T) == typeof(long))
            {
                return (T)(object)long.Parse(value);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to convert setting {Name} to type {Type}", name, typeof(T));
            return defaultValue;
        }
    }

    public virtual Task SetSettingAsync(string name, object value)
    {
        return SettingManager.SetForCurrentTenantAsync(name, value?.ToString());
    }
}