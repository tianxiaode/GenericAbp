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

        public static class Public
        {
            private const string PublicPrefix = GroupName + ".Public";
            public const string DefaultQuota = PublicPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = PublicPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = PublicPrefix + ".DefaultFileTypes";
        }

        public static class Shared
        {
            private const string SharedPrefix = GroupName + ".Shared";
            public const string DefaultQuota = SharedPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = SharedPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = SharedPrefix + ".DefaultFileTypes";
        }

        public static class Users
        {
            private const string PrivatePrefix = GroupName + ".Private";
            public const string DefaultQuota = PrivatePrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = PrivatePrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = PrivatePrefix + ".DefaultFileTypes";
        }

        public static class Virtual
        {
            private const string VirtualPrefix = GroupName + ".Virtual";
            public const string DefaultQuota = VirtualPrefix + ".DefaultQuota";
            public const string DefaultFileMaxSize = VirtualPrefix + ".DefaultFileMaxSize";
            public const string DefaultFileTypes = VirtualPrefix + ".DefaultFileTypes";
        }

        public static class Default
        {
            private const string DefaultPrefix = GroupName + ".Default";
            public const string UpdateEnable = DefaultPrefix + ".Update.Enable";
            public const string UpdateRetentionPeriod = DefaultPrefix + ".Update.RetentionPeriod";
            public const string UpdateFrequency = DefaultPrefix + ".Update.Frequency";
            public const string UpdateBatchSize = DefaultPrefix + ".Update.BatchSize";
            public const string CleanupEnable = DefaultPrefix + ".Cleanup.Enable";
            public const string CleanupFrequency = DefaultPrefix + ".Cleanup.Frequency";
            public const string CleanupBatchSize = DefaultPrefix + ".Cleanup.BatchSize";
        }

        public static class Temporary
        {
            private const string TemporaryPrefix = GroupName + ".Temporary";
            public const string UpdateEnable = TemporaryPrefix + ".Update.Enable";
            public const string UpdateRetentionPeriod = TemporaryPrefix + ".Update.RetentionPeriod";
            public const string UpdateFrequency = TemporaryPrefix + ".Update.Frequency";
            public const string UpdateBatchSize = TemporaryPrefix + ".Update.BatchSize";
            public const string CleanupEnable = TemporaryPrefix + ".Cleanup.Enable";
            public const string CleanupFrequency = TemporaryPrefix + ".Cleanup.Frequency";
            public const string CleanupBatchSize = TemporaryPrefix + ".Cleanup.BatchSize";
        }
    }
}