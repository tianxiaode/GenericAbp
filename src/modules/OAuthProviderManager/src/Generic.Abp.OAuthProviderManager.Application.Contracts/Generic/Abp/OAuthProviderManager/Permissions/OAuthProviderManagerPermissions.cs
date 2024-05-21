using Volo.Abp.Reflection;

namespace Generic.Abp.OAuthProviderManager.Permissions
{
    public class OAuthProviderManagerPermissions
    {
        public const string GroupName = "OAuthProviderManager";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OAuthProviderManagerPermissions));
        }

        public static class OAuthProviders
        {
            public const string Default = GroupName + ".OAuthProviders";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }
    }
}