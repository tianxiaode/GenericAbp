using Generic.Abp.Extensions.Settings;
using Generic.Abp.FileManagement.Localization;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Settings
{
    public class FileManagementSettingDefinitionProvider : SettingDefinitionExtensionsProvider<FileManagementResource>
    {
        protected override Dictionary<string, ISettingDefinitionExtensions> GetSettings()
        {
            return FileManagementSettings.Settings;
        }

        protected override string GetGroupName()
        {
            return FileManagementSettings.GroupName;
        }
    }
}