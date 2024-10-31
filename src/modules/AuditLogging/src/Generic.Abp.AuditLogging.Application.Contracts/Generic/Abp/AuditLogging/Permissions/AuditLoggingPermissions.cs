using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Reflection;

namespace Generic.Abp.AuditLogging.Permissions
{
    public class AuditLoggingPermissions
    {
        public const string GroupName = "AuditLogging";

        public const string AuditLogs = GroupName + ".AuditLogs";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(
                typeof(AuditLoggingPermissions));
        }
    }
}