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

            var fileInfoBasePermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.FileInfoBases.Default,
                L($"Permission:FileInfoBases"));
            fileInfoBasePermission.AddChild(FileManagementPermissions.FileInfoBases.ManageRetentionPolicy,
                L("Permission:ManageRetentionPolicy"));

            var foldersPermission = fileManagementGroup.AddPermission(FileManagementPermissions.Resources.Default,
                L($"Permission:Resources"));
            foldersPermission.AddChild(FileManagementPermissions.Resources.Create, L("Permission:Create"));
            foldersPermission.AddChild(FileManagementPermissions.Resources.Update, L("Permission:Edit"));
            foldersPermission.AddChild(FileManagementPermissions.Resources.Delete, L("Permission:Delete"));
            foldersPermission.AddChild(FileManagementPermissions.Resources.ManagePermissions,
                L("Permission:ChangePermissions"));

            var virtualPathsPermission = fileManagementGroup.AddPermission(
                FileManagementPermissions.VirtualPaths.Default,
                L($"Permission:VirtualPaths"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Create, L("Permission:Create"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Update, L("Permission:Edit"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.Delete, L("Permission:Delete"));
            virtualPathsPermission.AddChild(FileManagementPermissions.VirtualPaths.ManagePermissions,
                L("Permission:ChangePermissions"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileManagementResource>(name);
        }
    }
}