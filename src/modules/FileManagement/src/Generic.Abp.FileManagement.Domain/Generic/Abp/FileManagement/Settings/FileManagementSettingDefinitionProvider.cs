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
            context.Add(
                new SettingDefinition(
                    FileManagementSettings.StoragePath,
                    ResourceConsts.DefaultStoragePath,
                    L("Settings:StoragePath"),
                    L("Description:StoragePath"),
                    true // 可选，决定是否对外暴露
                ),
                //公共文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultFileTypes,
                    ResourceConsts.Public.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultFileMaxSize,
                    ResourceConsts.Public.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultQuota,
                    ResourceConsts.Public.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //共享文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultFileTypes,
                    ResourceConsts.Shared.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultFileMaxSize,
                    ResourceConsts.Shared.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultQuota,
                    ResourceConsts.Shared.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //用户文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultFileTypes,
                    ResourceConsts.User.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultFileMaxSize,
                    ResourceConsts.User.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultQuota,
                    ResourceConsts.User.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //虚拟文件夹默认配置
                new SettingDefinition(
                    FileManagementSettings.Virtual.DefaultFileTypes,
                    ResourceConsts.Virtual.DefaultFileTypes,
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Virtual.DefaultFileMaxSize,
                    ResourceConsts.Virtual.DefaultFileMaxSize,
                    L("Settings:DefaultFileMaxSize"),
                    L("Description:DefaultFileMaxSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Virtual.DefaultQuota,
                    ResourceConsts.Virtual.DefaultQuota,
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                //默认文件更新过期文件配置
                new SettingDefinition(
                    FileManagementSettings.Default.UpdateEnable,
                    "false",
                    L("Settings:Enable"),
                    L("Description:DefaultUpdateEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Default.UpdateRetentionPeriod,
                    ResourceConsts.Default.UpdateRetentionPeriod.ToString(),
                    L("Settings:RetentionPeriod"),
                    L("Description:DefaultUpdateRetentionPeriod"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Default.UpdateFrequency,
                    ResourceConsts.Default.UpdateFrequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:DefaultUpdateFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Default.UpdateBatchSize,
                    ResourceConsts.Default.UpdateBatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:DefaultUpdateBatchSize"),
                    true
                ),
                //默认文件清理过期文件配置
                new SettingDefinition(
                    FileManagementSettings.Default.CleanupEnable,
                    "false",
                    L("Settings:Enabled"),
                    L("Description:DefaultCleanupEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Default.CleanupFrequency,
                    ResourceConsts.Default.CleanupFrequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:DefaultCleanupFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Default.CleanupBatchSize,
                    ResourceConsts.Default.CleanupBatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:DefaultCleanupBatchSize"),
                    true
                ),
                //临时文件更新过期时间配置
                new SettingDefinition(
                    FileManagementSettings.Temporary.UpdateEnable,
                    "false",
                    L("Settings:Enable"),
                    L("Description:TemporaryUpdateEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Temporary.UpdateRetentionPeriod,
                    ResourceConsts.Default.UpdateRetentionPeriod.ToString(),
                    L("Settings:RetentionPeriod"),
                    L("Description:TemporaryUpdateRetentionPeriod"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Temporary.UpdateFrequency,
                    ResourceConsts.Default.UpdateFrequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:TemporaryUpdateFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Temporary.UpdateBatchSize,
                    ResourceConsts.Default.UpdateBatchSize.ToString(),
                    L("Settings:BatchSize"),
                    L("Description:TemporaryUpdateBatchSize"),
                    true
                ),
                //临时文件清理配置
                new SettingDefinition(
                    FileManagementSettings.Temporary.CleanupEnable,
                    "false",
                    L("Settings:Enable"),
                    L("Description:TemporaryCleanupEnable"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Temporary.CleanupFrequency,
                    ResourceConsts.Default.CleanupFrequency.ToString(),
                    L("Settings:Frequency"),
                    L("Description:TemporaryCleanupFrequency"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Temporary.CleanupBatchSize,
                    ResourceConsts.Default.CleanupBatchSize.ToString(),
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