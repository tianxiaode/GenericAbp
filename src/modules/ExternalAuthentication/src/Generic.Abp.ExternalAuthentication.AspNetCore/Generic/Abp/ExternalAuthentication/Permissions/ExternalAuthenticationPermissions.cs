using Volo.Abp.Reflection;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.ExternalAuthentication.Permissions;

public class ExternalAuthenticationPermissions
{
    public const string GroupName = SettingManagementPermissions.GroupName;
    public const string ExternalAuthenticationManagement = GroupName + ".ExternalAuthenticationManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(
            typeof(ExternalAuthenticationPermissions));
    }
}