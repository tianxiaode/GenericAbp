using Volo.Abp.Reflection;

namespace Generic.Abp.MenuManagement.Permissions
{
    public class MenuManagementPermissions
    {
        public const string GroupName = "MenuManagement";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(MenuManagementPermissions));
        }

        public static class Menus
        {
            public const string Default = GroupName + ".Menus";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}