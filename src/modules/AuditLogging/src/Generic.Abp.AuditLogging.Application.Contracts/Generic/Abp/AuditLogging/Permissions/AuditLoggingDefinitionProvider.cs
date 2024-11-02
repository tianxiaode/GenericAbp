using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.AuditLogging.Permissions;

public class AuditLoggingDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AuditLoggingPermissions.GroupName, L("Permission:AuditLogging"));
        myGroup.AddPermission(AuditLoggingPermissions.AuditLogs, L("Permission:AuditLogs"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}