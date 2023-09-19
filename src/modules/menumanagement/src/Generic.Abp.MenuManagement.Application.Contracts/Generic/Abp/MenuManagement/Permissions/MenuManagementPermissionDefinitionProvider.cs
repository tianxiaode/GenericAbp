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
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MenuManagementResource>(name);
        }
    }
}