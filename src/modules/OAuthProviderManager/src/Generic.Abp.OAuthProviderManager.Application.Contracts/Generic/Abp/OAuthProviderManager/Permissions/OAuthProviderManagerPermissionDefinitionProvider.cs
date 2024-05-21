using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.OAuthProviderManager.Permissions
{
    public class OAuthProviderManagerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(OAuthProviderManagerPermissions.GroupName,
                L("Permission:OAuthProviderManager"));

            var districtPermission = myGroup.AddPermission(OAuthProviderManagerPermissions.OAuthProviders.Default,
                L($"OAuthProviderManager.OAuthProviders.ManagePermissions"));
            districtPermission.AddChild(OAuthProviderManagerPermissions.OAuthProviders.ManagePermissions,
                L("Permission:ChangePermissions"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OAuthProviderManagerResource>(name);
        }
    }
}