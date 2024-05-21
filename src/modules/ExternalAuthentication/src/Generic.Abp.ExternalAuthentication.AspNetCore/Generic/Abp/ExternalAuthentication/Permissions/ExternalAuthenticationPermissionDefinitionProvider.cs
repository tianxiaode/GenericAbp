using Generic.Abp.ExternalAuthentication.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.ExternalAuthentication.Permissions;

public class ExternalAuthenticationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExternalAuthenticationPermissions.GroupName,
            L("Permission:ExternalAuthenticationProvider"));

        var districtPermission = myGroup.AddPermission(
            ExternalAuthenticationPermissions.ExternalAuthenticationProviders.Default,
            L($"ExternalAuthentication.Provider.ManagePermissions"));
        districtPermission.AddChild(ExternalAuthenticationPermissions.ExternalAuthenticationProviders.ManagePermissions,
            L("Permission:ChangePermissions"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExternalAuthenticationResource>(name);
    }
}