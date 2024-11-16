﻿namespace Generic.Abp.FileManagement.Settings
{
    public static class FileManagementSettings
    {
        public const string GroupName = "FileManagement";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        public static class Public
        {
            private const string PublicPrefix = GroupName + ".Public";
            public const string StoragePath = PublicPrefix + ".StoragePath";
            public const string DefaultQuota = PublicPrefix + ".DefaultQuota";
            public const string DefaultMaxFileSize = PublicPrefix + ".DefaultMaxFileSize";
            public const string DefaultFileTypes = PublicPrefix + ".DefaultFileTypes";
        }

        public static class Shared
        {
            private const string SharedPrefix = GroupName + ".Shared";
            public const string StoragePath = SharedPrefix + ".StoragePath";
            public const string DefaultQuota = SharedPrefix + ".DefaultQuota";
            public const string DefaultMaxFileSize = SharedPrefix + ".DefaultMaxFileSize";
            public const string DefaultFileTypes = SharedPrefix + ".DefaultFileTypes";
        }

        public static class Private
        {
            private const string PrivatePrefix = GroupName + ".Private";
            public const string StoragePath = PrivatePrefix + ".StoragePath";
            public const string DefaultQuota = PrivatePrefix + ".DefaultQuota";
            public const string DefaultMaxFileSize = PrivatePrefix + ".DefaultMaxFileSize";
            public const string DefaultFileTypes = PrivatePrefix + ".DefaultFileTypes";
        }
    }
}