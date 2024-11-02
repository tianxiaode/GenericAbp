using Generic.Abp.ExternalAuthentication.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.ExternalAuthentication.Permissions;

public class ExternalAuthenticationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.GetGroup(SettingManagementPermissions.GroupName);
        myGroup.AddPermission(ExternalAuthenticationPermissions.ExternalAuthenticationManagement,
            L("Permission:ExternalAuthenticationManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExternalAuthenticationResource>(name);
    }
}