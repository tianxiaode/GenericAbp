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

            var menusPermission = myGroup.AddPermission(MenuManagementPermissions.Menus.Default,
                L($"Permission:MenuManagement"));
            menusPermission.AddChild(MenuManagementPermissions.Menus.Create, L("Permission:Create"));
            menusPermission.AddChild(MenuManagementPermissions.Menus.Update, L("Permission:Edit"));
            menusPermission.AddChild(MenuManagementPermissions.Menus.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MenuManagementResource>(name);
        }
    }
}