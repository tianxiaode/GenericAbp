using Generic.Abp.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.FileManagement.Permissions
{
    public class FileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var fileManagementGroup =
                context.AddGroup(FileManagementPermissions.GroupName, L("Permission:FileManagement"));
            //设置管理
            fileManagementGroup.AddPermission(FileManagementPermissions.SettingManagement,
                L("Permission:SettingManagement"));

            var fileInfoBasePermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.FileInfoBases.Default,
                L($"Permission:FileInfoBases"));
            fileInfoBasePermission.AddChild(FileManagementPermissions.FileInfoBases.ManageRetentionPolicy,
                L("Permission:ManageRetentionPolicy"));

            var resourcesPermission = fileManagementGroup.AddPermission(FileManagementPermissions.Resources.Default,
                L($"Permission:Resources"));
            resourcesPermission.AddChild(FileManagementPermissions.Resources.Create, L("Permission:Create"));
            resourcesPermission.AddChild(FileManagementPermissions.Resources.Update, L("Permission:Edit"));
            resourcesPermission.AddChild(FileManagementPermissions.Resources.Delete, L("Permission:Delete"));
            resourcesPermission.AddChild(FileManagementPermissions.Resources.ManageConfigurations,
                L("Permission:ManageConfigurations"));
            resourcesPermission.AddChild(FileManagementPermissions.Resources.ManagePermissions,
                L("Permission:ChangePermissions"));

            var userFoldersPermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.UserFolders.Default,
                L($"Permission:UserFolders"));
            userFoldersPermission.AddChild(FileManagementPermissions.UserFolders.Create, L("Permission:Create"));
            userFoldersPermission.AddChild(FileManagementPermissions.UserFolders.Update, L("Permission:Edit"));
            userFoldersPermission.AddChild(FileManagementPermissions.UserFolders.Delete, L("Permission:Delete"));

            var virtualPathsPermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.VirtualPaths.Default,
                L($"Permission:VirtualPaths"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Create, L("Permission:Create"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Update, L("Permission:Edit"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Delete, L("Permission:Delete"));

            var externalSharesPermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.ExternalShares.Default,
                L($"Permission:ExternalShares"));
            externalSharesPermission.AddChild(FileManagementPermissions.ExternalShares.Create, L("Permission:Create"));
            externalSharesPermission.AddChild(FileManagementPermissions.ExternalShares.Update, L("Permission:Edit"));
            externalSharesPermission.AddChild(FileManagementPermissions.ExternalShares.Delete, L("Permission:Delete"));
        }


        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileManagementResource>(name);
        }
    }
}