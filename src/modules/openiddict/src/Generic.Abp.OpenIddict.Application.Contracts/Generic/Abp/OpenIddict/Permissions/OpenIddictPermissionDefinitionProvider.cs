using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.OpenIddict.Permissions
{
    public class OpenIddictPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var openIddictGroup = context.AddGroup(OpenIddictPermissions.GroupName, L("Permission:OpenIddict"));

            var applicationsPermission = openIddictGroup.AddPermission(OpenIddictPermissions.Applications.Default,
                L("Permission:Applications"));
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Create, L("Permission:Create"));
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Update, L("Permission:Edit"));
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Delete, L("Permission:Delete"));
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.ManagePermissions,
                L("Permission:ManagePermissions"));

            var scopesPermission =
                openIddictGroup.AddPermission(OpenIddictPermissions.Scopes.Default, L("Permission:Scopes"));
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Create, L("Permission:Create"));
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Update, L("Permission:Edit"));
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OpenIddictResource>(name);
        }
    }
}