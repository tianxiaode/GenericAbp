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
    }
}