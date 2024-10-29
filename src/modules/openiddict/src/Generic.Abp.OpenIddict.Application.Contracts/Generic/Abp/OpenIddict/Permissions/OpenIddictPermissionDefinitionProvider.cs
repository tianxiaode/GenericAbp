using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.OpenIddict.Permissions
{
    public class OpenIddictPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var openIddictGroup = context.AddGroup(OpenIddictPermissions.GroupName, L("Permission:OpenIddict"));

            var applicationsPermission = openIddictGroup.AddPermission(OpenIddictPermissions.Applications.Default,
                L("Permission:Applications"), MultiTenancySides.Host);
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Create, L("Permission:Create"),
                MultiTenancySides.Host);
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Update, L("Permission:Edit"),
                MultiTenancySides.Host);
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.Delete, L("Permission:Delete"),
                MultiTenancySides.Host);
            applicationsPermission.AddChild(OpenIddictPermissions.Applications.ManagePermissions,
                L("Permission:ManagePermissions"), MultiTenancySides.Host);

            var scopesPermission =
                openIddictGroup.AddPermission(OpenIddictPermissions.Scopes.Default, L("Permission:Scopes"),
                    MultiTenancySides.Host);
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Create, L("Permission:Create"),
                MultiTenancySides.Host);
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Update, L("Permission:Edit"),
                MultiTenancySides.Host);
            scopesPermission.AddChild(OpenIddictPermissions.Scopes.Delete, L("Permission:Delete"),
                MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OpenIddictResource>(name);
        }
    }
}