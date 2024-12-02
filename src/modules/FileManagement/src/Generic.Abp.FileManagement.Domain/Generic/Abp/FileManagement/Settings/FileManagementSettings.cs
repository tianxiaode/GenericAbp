namespace Generic.Abp.FileManagement.Settings
{
    public static class FileManagementSettings
    {
        public const string GroupName = "FileManagement";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        public const string StoragePath = GroupName + ".StoragePath";
        public const string TempPath = GroupName + ".TempPath";
        public const string FolderCopyMaxNodeCount = GroupName + ".FolderCopyMaxNodeCount";
        public const string EnablePersonalFolderForRoles = GroupName + ".EnablePersonalFolderForRoles";
        public const string ExpirationDateOfExternalShared = GroupName + ".ExpirationDateOfExternalShared";

        public static class PublicFolder
        {
            private const string PublicFolderPrefix = GroupName + ".PublicFolder";
            public const string DefaultQuota = PublicFolderPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = PublicFolderPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = PublicFolderPrefix + ".DefaultFileTypes";
        }

        public static class SharedFolder
        {
            private const string SharedFolderPrefix = GroupName + ".SharedFolder";
            public const string DefaultQuota = SharedFolderPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = SharedFolderPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = SharedFolderPrefix + ".DefaultFileTypes";
        }

        public static class UserFolder
        {
            private const string UserFolderPrefix = GroupName + ".UserFolder";
            public const string DefaultQuota = UserFolderPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = UserFolderPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = UserFolderPrefix + ".DefaultFileTypes";
        }

        public static class VirtualPath
        {
            private const string VirtualPathPrefix = GroupName + ".Virtual";
            public const string DefaultQuota = VirtualPathPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = VirtualPathPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = VirtualPathPrefix + ".DefaultFileTypes";
        }

        public static class ParticipantIsolationFolder
        {
            private const string ParticipantIsolationFolderPrefix = GroupName + ".ParticipantIsolationRootFolder";
            public const string DefaultQuota = ParticipantIsolationFolderPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = ParticipantIsolationFolderPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = ParticipantIsolationFolderPrefix + ".DefaultFileTypes";
        }

        public static class DefaultFile
        {
            private const string DefaultPrefix = GroupName + ".DefaultFile";

            public static class Update
            {
                private const string UpdatePrefix = DefaultPrefix + ".Update";
                public const string Enable = UpdatePrefix + ".Enable";
                public const string RetentionPeriod = UpdatePrefix + ".RetentionPeriod";
                public const string Frequency = UpdatePrefix + ".Frequency";
                public const string BatchSize = UpdatePrefix + ".BatchSize";
            }

            public static class Cleanup
            {
                private const string CleanupPrefix = DefaultPrefix + ".Cleanup";
                public const string Enable = CleanupPrefix + ".Enable";
                public const string Frequency = CleanupPrefix + ".Frequency";
                public const string BatchSize = CleanupPrefix + ".BatchSize";
            }
        }

        public static class TemporaryFile
        {
            private const string TemporaryFilePrefix = GroupName + ".TemporaryFile";

            public static class Update
            {
                private const string UpdatePrefix = TemporaryFilePrefix + ".Update";
                public const string Enable = UpdatePrefix + ".Enable";
                public const string RetentionPeriod = UpdatePrefix + ".RetentionPeriod";
                public const string Frequency = UpdatePrefix + ".Frequency";
                public const string BatchSize = UpdatePrefix + ".BatchSize";
            }

            public static class Cleanup
            {
                private const string CleanupPrefix = TemporaryFilePrefix + ".Cleanup";
                public const string Enable = CleanupPrefix + ".Enable";
                public const string Frequency = CleanupPrefix + ".Frequency";
                public const string BatchSize = CleanupPrefix + ".BatchSize";
            }
        }
    }
}