using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.EventHandlers;

public class ResourceDeletedEventHandler(
    IFileInfoBaseRepository fileInfoBaseRepository,
    IResourceRepository resourceRepository,
    ISettingManager settingManager)
    : IDistributedEventHandler<ResourceDeletedEto>
{
    protected IFileInfoBaseRepository FileInfoBaseRepository { get; } = fileInfoBaseRepository;
    protected IResourceRepository ResourceRepository { get; } = resourceRepository;
    protected ISettingManager SettingManager { get; } = settingManager;

    public async Task HandleEventAsync(ResourceDeletedEto eventData)
    {
        if (!eventData.FileInfoBaseId.HasValue)
        {
            return;
        }

        var isReferenced = await ResourceRepository.AnyAsync(r => r.FileInfoBaseId == eventData.FileInfoBaseId.Value);

        if (isReferenced)
        {
            return;
            // 添加到清理队列或标记过期
        }

        var fileInfo = await FileInfoBaseRepository.GetAsync(eventData.FileInfoBaseId.Value);
        int period;
        // 根据文件保留策略设置不同的过期时间
        switch (fileInfo.RetentionPolicy)
        {
            case FileRetentionPolicy.Default:
            {
                var periodStr =
                    await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.DefaultRetentionPeriod);
                int.TryParse(periodStr, out period);
                break;
            }
            case FileRetentionPolicy.Temporary:
            {
                var periodStr =
                    await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings
                        .TemporaryRetentionPeriod);
                int.TryParse(periodStr, out period);
                break;
            }
            case FileRetentionPolicy.Retain:
            default:
                return;
        }


        fileInfo.SetExpireAt(DateTime.UtcNow.AddDays(period));
        await FileInfoBaseRepository.UpdateAsync(fileInfo);
    }
}