﻿namespace Generic.Abp.FileManagement.Resources;

public static class ResourceConsts
{
    public const string PublicRootFolderName = "PUBLIC";
    public const string UsersRootFolderName = "USERS";
    public const string SharedRootFolderName = "SHARED";
    public const string VirtualRootFolderName = "VIRTUAL";

    public static string DefaultStoragePath = "/files";
    public static int FolderCopyMaxNodeCount = 500;

    public static class PublicFolder
    {
        public static string DefaultFileTypes =
            ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z";

        public static string DefaultFileMaxSize = "50MB";
        public static string DefaultQuota = "1GB";
    }

    public static class SharedFolder
    {
        public const string DefaultFileTypes =
            ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z";

        public static string DefaultFileMaxSize = "50MB";
        public static string DefaultQuota = "1GB";
    }

    public static class UserFolder
    {
        public static string DefaultFileTypes =
            ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z";

        public static string DefaultFileMaxSize = "2MB";
        public static string DefaultQuota = "200MB";
    }

    public static class VirtualPath
    {
        public static string DefaultFileTypes =
            ".docx,.pdf,.xlsx,.pptx,.txt,.csv,.xml,.json,.html,.js,.css,.md,.jpg,.png,.gif,.zip,.rar,.7z";

        public static string DefaultFileMaxSize = "2MB";
        public static string DefaultQuota = "200MB";
    }

    public static class DefaultFile
    {
        public static class Update
        {
            public const bool Enable = true;
            public const int RetentionPeriod = 30;
            public const int BatchSize = 1000;
            public const int Frequency = 3600 * 36; //36 hours
        }

        public static class Cleanup
        {
            public const bool Enable = true;
            public const int BatchSize = 1000;
            public const int Frequency = 3600 * 24 * 15; //15 days
        }
    }

    public static class TemporaryFile
    {
        public static class Update
        {
            public const bool Enable = true;
            public const int RetentionPeriod = 30;
            public const int BatchSize = 1000;
            public const int Frequency = 3600 * 12; //12 hours
        }

        public static class Cleanup
        {
            public const bool Enable = true;
            public const int BatchSize = 1000;
            public const int Frequency = 3600 * 24 * 7; //7 days
        }
    }


    public static int NameMaxLength { get; set; } = 256;
    public static int ProviderNameMaxLength { get; set; } = 16;
    public static int ProviderKeyMaxLength { get; set; } = 64;
    public static int AllowedFileTypesMaxLength { get; set; } = 2048;
    public static int MaxDepth { get; set; } = 32;

    public static int CodeUnitLength { get; set; } = 6;

    public static int CodeMaxLength => GetCodeLength(32);

    public static int GetCodeLength(int level)
    {
        return CodeUnitLength * level + level - 1;
    }
}