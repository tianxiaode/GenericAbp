using System;
using System.Threading.Tasks;
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
            PublicFolder = await GetPublicFolderSettingAsync(),
            SharedFolder = await GetSharedFolderSettingAsync(),
            UsersFolder = await GetUserFolderSettingAsync(),
            VirtualPath = await GetVirtualPathSettingAsync(),
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
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultQuota, settingResult.PublicFolder.Quota);
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileMaxSize,
            settingResult.PublicFolder.FileMaxSize);
        await SetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileTypes,
            settingResult.PublicFolder.FileTypes);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultQuota, settingResult.SharedFolder.Quota);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileMaxSize,
            settingResult.SharedFolder.FileMaxSize);
        await SetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileTypes,
            settingResult.SharedFolder.FileTypes);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultQuota, settingResult.UsersFolder.Quota);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultFileMaxSize,
            settingResult.UsersFolder.FileMaxSize);
        await SetSettingAsync(FileManagementSettings.UserFolder.DefaultFileTypes, settingResult.UsersFolder.FileTypes);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultQuota, settingResult.VirtualPath.Quota);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileMaxSize,
            settingResult.VirtualPath.FileMaxSize);
        await SetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileTypes, settingResult.VirtualPath.FileTypes);
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
        return await GetSettingAsync(FileManagementSettings.StoragePath, ResourceConsts.DefaultStoragePath);
    }

    public virtual async Task<int> GetFolderCopyMaxNodeCountAsync()
    {
        return await GetSettingAsync(FileManagementSettings.FolderCopyMaxNodeCount,
            ResourceConsts.FolderCopyMaxNodeCount);
    }

    public virtual async Task<FolderSetting> GetPublicFolderSettingAsync()
    {
        return new FolderSetting()
        {
            Quota = await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultQuota,
                ResourceConsts.PublicFolder.DefaultQuota),
            FileMaxSize = await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileMaxSize,
                ResourceConsts.PublicFolder.DefaultFileMaxSize),
            FileTypes = await GetSettingAsync(FileManagementSettings.PublicFolder.DefaultFileTypes,
                ResourceConsts.PublicFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetSharedFolderSettingAsync()
    {
        return new FolderSetting()
        {
            Quota = await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultQuota,
                ResourceConsts.SharedFolder.DefaultQuota),
            FileMaxSize = await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileMaxSize,
                ResourceConsts.SharedFolder.DefaultFileMaxSize),
            FileTypes = await GetSettingAsync(FileManagementSettings.SharedFolder.DefaultFileTypes,
                ResourceConsts.SharedFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetUserFolderSettingAsync()
    {
        return new FolderSetting()
        {
            Quota = await GetSettingAsync(FileManagementSettings.UserFolder.DefaultQuota,
                ResourceConsts.UserFolder.DefaultQuota),
            FileMaxSize = await GetSettingAsync(FileManagementSettings.UserFolder.DefaultFileMaxSize,
                ResourceConsts.UserFolder.DefaultFileMaxSize),
            FileTypes = await GetSettingAsync(FileManagementSettings.UserFolder.DefaultFileTypes,
                ResourceConsts.UserFolder.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetVirtualPathSettingAsync()
    {
        return new FolderSetting()
        {
            Quota = await GetSettingAsync(FileManagementSettings.VirtualPath.DefaultQuota,
                ResourceConsts.VirtualPath.DefaultQuota),
            FileMaxSize = await GetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileMaxSize,
                ResourceConsts.VirtualPath.DefaultFileMaxSize),
            FileTypes = await GetSettingAsync(FileManagementSettings.VirtualPath.DefaultFileTypes,
                ResourceConsts.VirtualPath.DefaultFileTypes)
        };
    }

    public virtual async Task<FolderSetting> GetParticipantIsolationFolderSettingAsync()
    {
        return new FolderSetting()
        {
            Quota = await GetSettingAsync(FileManagementSettings.ParticipantIsolationFolder.DefaultQuota,
                ResourceConsts.VirtualPath.DefaultQuota),
            FileMaxSize = await GetSettingAsync(
                FileManagementSettings.ParticipantIsolationFolder.DefaultFileMaxSize,
                ResourceConsts.VirtualPath.DefaultFileMaxSize),
            FileTypes = await GetSettingAsync(FileManagementSettings.ParticipantIsolationFolder.DefaultFileTypes,
                ResourceConsts.VirtualPath.DefaultFileTypes)
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
                ResourceConsts.DefaultFile.Update.Enable),
            RetentionPeriod = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.RetentionPeriod,
                ResourceConsts.DefaultFile.Update.RetentionPeriod),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.Frequency,
                ResourceConsts.DefaultFile.Update.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Update.BatchSize,
                ResourceConsts.DefaultFile.Update.BatchSize),
        };
    }

    public virtual async Task<FileCleanupSetting> GetDefaultFileCleanupSettingAsync()
    {
        return new FileCleanupSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Enable,
                ResourceConsts.DefaultFile.Cleanup.Enable),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Frequency,
                ResourceConsts.DefaultFile.Cleanup.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.BatchSize,
                ResourceConsts.DefaultFile.Cleanup.BatchSize),
        };
    }

    public virtual async Task<FileUpdateSetting> GetTemporaryFileUpdateSettingAsync()
    {
        return new FileUpdateSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.Enable,
                ResourceConsts.TemporaryFile.Update.Enable),
            RetentionPeriod = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.RetentionPeriod,
                ResourceConsts.TemporaryFile.Update.RetentionPeriod),
            Frequency = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.Frequency,
                ResourceConsts.TemporaryFile.Update.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.TemporaryFile.Update.BatchSize,
                ResourceConsts.TemporaryFile.Update.BatchSize),
        };
    }

    public virtual async Task<FileCleanupSetting> GetTemporaryFileCleanupSettingAsync()
    {
        return new FileCleanupSetting()
        {
            Enable = await GetSettingAsync(FileManagementSettings.TemporaryFile.Cleanup.Enable,
                ResourceConsts.TemporaryFile.Cleanup.Enable),
            Frequency = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.Frequency,
                ResourceConsts.TemporaryFile.Cleanup.Frequency),
            BatchSize = await GetSettingAsync(FileManagementSettings.DefaultFile.Cleanup.BatchSize,
                ResourceConsts.TemporaryFile.Cleanup.BatchSize),
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