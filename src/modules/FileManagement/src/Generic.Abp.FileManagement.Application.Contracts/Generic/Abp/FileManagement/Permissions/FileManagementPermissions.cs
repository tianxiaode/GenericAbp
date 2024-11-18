using Volo.Abp.Reflection;

namespace Generic.Abp.FileManagement.Permissions
{
    public class FileManagementPermissions
    {
        public const string GroupName = "FileManagement";

        public static class Folders
        {
            public const string Default = GroupName + ".Folders";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class VirtualPaths
        {
            public const string Default = GroupName + ".VirtualPaths";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileManagementPermissions));
        }
    }
}