using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;

namespace Generic.Abp.IdentityServer.Permissions
{
    public class IdentityServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityServerGroup = context.AddGroup(IdentityServerPermissions.GroupName, L("Permission:IdentityServer"));

            var identityResourcesPermission = identityServerGroup.AddPermission(IdentityServerPermissions.IdentityResources.Default, L("Permission:IdentityResources"));
            identityResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Create, L("Permission:Create"));
            identityResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Update, L("Permission:Edit"));
            identityResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Delete, L("Permission:Delete"));
            identityResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.ManagePermissions, L("Permission:ChangePermissions"));

            var clientsPermission = identityServerGroup.AddPermission(IdentityServerPermissions.Clients.Default, L("Permission:Clients"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Create, L("Permission:Create"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Update, L("Permission:Edit"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Delete, L("Permission:Delete"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.ManagePermissions, L("Permission:ChangePermissions"));

            var apiResourcesPermission = identityServerGroup.AddPermission(IdentityServerPermissions.ApiResources.Default, L("Permission:ApiResources"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Create, L("Permission:Create"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Update, L("Permission:Edit"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Delete, L("Permission:Delete"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.ManagePermissions, L("Permission:ChangePermissions"));

            var apiScopesPermission = identityServerGroup.AddPermission(IdentityServerPermissions.ClaimTypes.Default, L("Permission:ClaimTypes"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}