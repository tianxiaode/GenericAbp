using Generic.Abp.FileManagement.Localization;
using System.Text.RegularExpressions;
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

            var foldersPermission = fileManagementGroup.AddPermission(FileManagementPermissions.Folders.Default,
                L($"Permission:Folders"));
            foldersPermission.AddChild(FileManagementPermissions.Folders.Create, L("Permission:Create"));
            foldersPermission.AddChild(FileManagementPermissions.Folders.Update, L("Permission:Edit"));
            foldersPermission.AddChild(FileManagementPermissions.Folders.Delete, L("Permission:Delete"));
            foldersPermission.AddChild(FileManagementPermissions.Folders.ManagePermissions,
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