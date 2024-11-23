using Generic.Abp.FileManagement.Localization;
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
                    "/files",
                    L("Settings:StoragePath"),
                    L("Description:StoragePath"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.DefaultRetentionPeriod,
                    "30",
                    L("Settings:DefaultRetentionPeriod"),
                    L("Description:DefaultRetentionPeriod"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.TemporaryRetentionPeriod,
                    "7",
                    L("Settings:TemporaryRetentionPeriod"),
                    L("Description:TemporaryRetentionPeriod"),
                    true // 可选，决定是否对外暴露
                ),
                // new SettingDefinition(
                //     FileManagementSettings.TempPath,
                //     "/files",
                //     L("Settings:TempPath"),
                //     L("Description:TempPath"),
                //     true // 可选，决定是否对外暴露
                // ),
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultFileTypes,
                    ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z",
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultMaxFileSize,
                    "2mb",
                    L("Settings:DefaultMaxFileSize"),
                    L("Description:DefaultMaxFileSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Public.DefaultQuota,
                    "2mb",
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultFileTypes,
                    ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z",
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultMaxFileSize,
                    "2mb",
                    L("Settings:DefaultMaxFileSize"),
                    L("Description:DefaultMaxFileSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Shared.DefaultQuota,
                    "2mb",
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultFileTypes,
                    ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z",
                    L("Settings:DefaultFileTypes"),
                    L("Description:DefaultFileTypes"),
                    true // 可选，决定是否对外暴露
                ),
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultMaxFileSize,
                    "2mb",
                    L("Settings:DefaultMaxFileSize"),
                    L("Description:DefaultMaxFileSize"),
                    true
                ),
                new SettingDefinition(
                    FileManagementSettings.Users.DefaultQuota,
                    "2mb",
                    L("Settings:DefaultQuota"),
                    L("Description:DefaultQuota"),
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