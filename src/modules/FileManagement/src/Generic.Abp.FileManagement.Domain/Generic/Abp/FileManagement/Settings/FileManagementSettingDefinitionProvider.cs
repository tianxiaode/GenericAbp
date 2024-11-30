using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.FileManagement.Settings
{
    public class FileManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            /* Define module settings here.
             * Use names from FileManagementSettings class.
             */
            //存储路径
            context.Add(
                new SettingDefinition(
                    FileManagementSettings.StoragePath,
                    ResourceConsts.DefaultStoragePath,
                    L("Settings:StoragePath"),
                    L("Description:StoragePath"),
                    true // 可选，决定是否对外暴露
                ),
                //启用个人文件夹的角色
                new SettingDefinition(
                    FileManagementSettings.EnablePersonalFolderForRoles,
                    "",
                    L("Settings:EnablePersonalFolderForRoles"),
                    L("Description:EnablePersonalFolderForRoles"),
                    true // 可选，决定是否对外暴露
                ),
                //复制文件夹最大节点数量
                new SettingDefinition(
                    FileManagementSettings.FolderCopyMaxNodeCount,
                    ResourceConsts.FolderCopyMaxNodeCount.ToString(),
                    L("Settings:StoragePath"),
                    L("Description:StoragePath"),
                    true // 可选，决定是否对外暴露
                ),
                //公共文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.PublicFolder.DefaultFileTypes,
                    ResourceConsts.PublicFolder.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.PublicFolder.DefaultFileMaxSize,
                    ResourceConsts.PublicFolder.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.PublicFolder.DefaultQuota,
                    ResourceConsts.PublicFolder.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //共享文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.SharedFolder.DefaultFileTypes,
                    ResourceConsts.SharedFolder.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.SharedFolder.DefaultFileMaxSize,
                    ResourceConsts.SharedFolder.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.SharedFolder.DefaultQuota,
                    ResourceConsts.SharedFolder.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //用户文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.UserFolder.DefaultFileTypes,
                    ResourceConsts.UserFolder.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.UserFolder.DefaultFileMaxSize,
                    ResourceConsts.UserFolder.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.UserFolder.DefaultQuota,
                    ResourceConsts.UserFolder.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //虚拟文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.VirtualPath.DefaultFileTypes,
                    ResourceConsts.VirtualPath.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.VirtualPath.DefaultFileMaxSize,
                    ResourceConsts.VirtualPath.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.VirtualPath.DefaultQuota,
                    ResourceConsts.VirtualPath.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //参与者隔离文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.ParticipantIsolationFolder.DefaultFileTypes,
                    ResourceConsts.ParticipantIsolationFolder.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.ParticipantIsolationFolder.DefaultFileMaxSize,
                    ResourceConsts.ParticipantIsolationFolder.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.ParticipantIsolationFolder.DefaultQuota,
                    ResourceConsts.ParticipantIsolationFolder.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //默认文件更新过期文件配置
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Update.Enable,
                    ResourceConsts.DefaultFile.Update.Enable.ToString(),
                    L("Settings:Enable"),
                    L("Description:DefaultUpdateEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Update.RetentionPeriod,
                    ResourceConsts.DefaultFile.Update.RetentionPeriod.ToString(),
                    L("Settings:RetentionPeriod"),
                    L("Description:DefaultUpdateRetentionPeriod"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Update.Frequency,
                    ResourceConsts.DefaultFile.Update.Frequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:DefaultUpdateFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Update.BatchSize,
                    ResourceConsts.DefaultFile.Update.BatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:DefaultUpdateBatchSize"),
                    true
                ),
                //默认文件清理过期文件配置
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Cleanup.Enable,
                    ResourceConsts.DefaultFile.Update.Enable.ToString(),
                    L("Settings:Enabled"),
                    L("Description:DefaultCleanupEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Cleanup.Frequency,
                    ResourceConsts.DefaultFile.Cleanup.Frequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:DefaultCleanupFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultFile.Cleanup.BatchSize,
                    ResourceConsts.DefaultFile.Cleanup.BatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:DefaultCleanupBatchSize"),
                    true
                ),
                //临时文件更新过期时间配置
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Update.Enable,
                    ResourceConsts.TemporaryFile.Update.Enable.ToString(),
                    L("Settings:Enable"),
                    L("Description:TemporaryUpdateEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Update.RetentionPeriod,
                    ResourceConsts.TemporaryFile.Update.RetentionPeriod.ToString(),
                    L("Settings:RetentionPeriod"),
                    L("Description:TemporaryUpdateRetentionPeriod"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Update.Frequency,
                    ResourceConsts.TemporaryFile.Update.Frequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:TemporaryUpdateFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Update.BatchSize,
                    ResourceConsts.TemporaryFile.Update.BatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:TemporaryUpdateBatchSize"),
                    true
                ),
                //临时文件清理配置
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Cleanup.Enable,
                    ResourceConsts.TemporaryFile.Cleanup.Enable.ToString(),
                    L("Settings:Enable"),
                    L("Description:TemporaryCleanupEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Cleanup.Frequency,
                    ResourceConsts.TemporaryFile.Cleanup.Frequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:TemporaryCleanupFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryFile.Cleanup.BatchSize,
                    ResourceConsts.TemporaryFile.Cleanup.BatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:TemporaryCleanupBatchSize"),
                    true
                )
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileManagementResource>(name);
        }
    }
}