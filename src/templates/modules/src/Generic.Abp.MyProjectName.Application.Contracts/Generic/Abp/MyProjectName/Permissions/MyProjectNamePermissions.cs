using Volo.Abp.Reflection;

namespace Generic.Abp.MyProjectName.Permissions
{
    public class MyProjectNamePermissions
    {
        public const string GroupName = "MyProjectName";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(MyProjectNamePermissions));
        }
    }
}