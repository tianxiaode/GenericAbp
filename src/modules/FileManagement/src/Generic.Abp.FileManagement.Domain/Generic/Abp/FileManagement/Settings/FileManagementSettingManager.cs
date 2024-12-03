using Generic.Abp.Extensions.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Events;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.SettingManagement;
using System;

namespace Generic.Abp.FileManagement.Settings;

public class FileManagementSettingManager(ISettingManager settingManager, IDistributedEventBus distributedEventBus)
    : SettingManagerBase(settingManager)
{
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;

    public override async Task UpdateAsync(Dictionary<string, object> newSettings, Guid? tenantId = null)
    {
        await base.UpdateAsync(newSettings, tenantId);
        await DistributedEventBus.PublishAsync(new FileCleanupSettingChangeEto(tenantId));
    }

    public virtual async Task<Dictionary<string, CleanupOrUpdateSetting>> GetAllCleanupOrUpdateSettingsAsync()
    {
        var settings = new Dictionary<string, CleanupOrUpdateSetting>();
        foreach (var prefix in FileManagementSettings.CleanupOrUpdateSettingPrefixes)
        {
            foreach (var type in FileManagementSettings.CleanupOrUpdateSettingTypes)
            {
                var settingName = $"{prefix}.{type}";
                settings.Add(settingName, new CleanupOrUpdateSetting(
                    await GetSettingAsync<bool>($"{settingName}.Enabled"),
                    await GetSettingAsync<long>($"{settingName}.Frequency"),
                    await GetSettingAsync<int>($"{settingName}.BatchSize"),
                    type == "Cleanup" ? null : await GetSettingAsync<int>($"{settingName}.RetentionPeriod")
                ));
            }
        }

        return settings;
    }

    public virtual async Task<FolderSetting> GetFolderSettingAsync(string prefix)
    {
        var settings = new FolderSetting(
            await GetSettingAsync<long>(prefix + ".StorageQuota"),
            await GetSettingAsync<long>(prefix + ".MaxFileSize"),
            await GetSettingAsync<string>(prefix + ".AllowFileTypes")
        );
        return settings;
    }

    public virtual async Task<int> GetExpirationDateOfExternalSharedAsync()
    {
        return await GetSettingAsync<int>(FileManagementSettings.ExpirationDateOfExternalShared);
    }

    protected override Dictionary<string, ISettingDefinitionExtensions> GetSettings()
    {
        return FileManagementSettings.Settings;
    }

    protected override string GetGroupName()
    {
        return FileManagementSettings.GroupName;
    }
}