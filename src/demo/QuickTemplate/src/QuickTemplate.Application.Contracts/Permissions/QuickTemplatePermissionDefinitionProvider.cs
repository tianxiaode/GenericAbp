using QuickTemplate.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace QuickTemplate.Permissions;

public class QuickTemplatePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(QuickTemplatePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(QuickTemplatePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<QuickTemplateResource>(name);
    }
}
