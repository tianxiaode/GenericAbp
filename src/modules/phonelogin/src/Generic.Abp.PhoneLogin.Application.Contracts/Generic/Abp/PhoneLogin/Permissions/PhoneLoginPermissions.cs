using Volo.Abp.Reflection;

namespace Generic.Abp.PhoneLogin.Permissions
{
    public class PhoneLoginPermissions
    {
        public const string GroupName = "PhoneLogin";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PhoneLoginPermissions));
        }
    }
}