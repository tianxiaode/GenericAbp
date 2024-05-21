using Volo.Abp.Reflection;

namespace Generic.Abp.ExternalAuthentication.Permissions;

public class ExternalAuthenticationPermissions
{
    public const string GroupName = "ExternalAuthentication";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(
            typeof(ExternalAuthenticationPermissionDefinitionProvider));
    }

    public static class ExternalAuthenticationProviders
    {
        public const string Default = GroupName + ".Providers";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }
}