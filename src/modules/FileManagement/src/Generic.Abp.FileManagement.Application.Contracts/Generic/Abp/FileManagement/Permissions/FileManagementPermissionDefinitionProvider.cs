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
            fileManagementGroup.AddPermission(FileManagementPermissions.AdministratorPermission,
                L("Permission:Administrator"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileManagementResource>(name);
        }
    }
}