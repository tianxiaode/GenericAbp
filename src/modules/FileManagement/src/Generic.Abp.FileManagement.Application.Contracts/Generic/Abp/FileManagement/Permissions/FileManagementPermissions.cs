using Volo.Abp.Reflection;

namespace Generic.Abp.FileManagement.Permissions
{
    public class FileManagementPermissions
    {
        public const string GroupName = "FileManagement";
        public const string SettingManagement = GroupName + ".SettingManagement";

        public static class FileInfoBases
        {
            public const string Default = GroupName + ".FileInfoBases";
            public const string ManageRetentionPolicy = Default + ".ManageRetentionPolicy";
        }

        public static class Resources
        {
            public const string Default = GroupName + ".Folders";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageConfigurations = Default + ".ManageConfigureds";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class UserFolders
        {
            public const string Default = GroupName + ".UserFolders";
            public const string ManageEnable = Default + ".ManageEnable";
            public const string ManageConfigurations = Default + ".ManageConfigureds";
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