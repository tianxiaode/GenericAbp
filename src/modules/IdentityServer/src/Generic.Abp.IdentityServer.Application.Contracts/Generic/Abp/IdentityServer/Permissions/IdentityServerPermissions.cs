using Volo.Abp.Reflection;

namespace Generic.Abp.IdentityServer.Permissions
{
    public class IdentityServerPermissions
    {
        public const string GroupName = "IdentityServer";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityServerPermissions));
        }

        public static class IdentityResources
        {
            public const string Default = GroupName + ".IdentityResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class ApiResources
        {
            public const string Default = GroupName + ".ApiResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class ApiScopes
        {
            public const string Default = GroupName + ".ApiScopes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }
    }
}