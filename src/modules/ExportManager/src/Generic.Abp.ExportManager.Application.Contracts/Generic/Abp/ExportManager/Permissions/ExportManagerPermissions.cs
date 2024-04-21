using Volo.Abp.Reflection;

namespace Generic.Abp.ExportManager.Permissions
{
    public class ExportManagerPermissions
    {
        public const string GroupName = "ExportManager";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ExportManagerPermissions));
        }
    }
}