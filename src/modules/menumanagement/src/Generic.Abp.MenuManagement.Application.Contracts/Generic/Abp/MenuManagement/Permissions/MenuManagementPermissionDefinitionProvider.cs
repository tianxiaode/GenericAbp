using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.MenuManagement.Permissions
{
    public class MenuManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(MenuManagementPermissions.GroupName, L("Permission:MenuManagement"));

            var districtPermission = myGroup.AddPermission(MenuManagementPermissions.Menus.Default,
                L($"Permission.MenuManagement"));
            districtPermission.AddChild(MenuManagementPermissions.Menus.Create, L("Permission:Create"));
            districtPermission.AddChild(MenuManagementPermissions.Menus.Update, L("Permission:Edit"));
            districtPermission.AddChild(MenuManagementPermissions.Menus.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MenuManagementResource>(name);
        }
    }
}