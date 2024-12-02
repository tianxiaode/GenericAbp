namespace Generic.Abp.FileManagement;

public class FileManagementDefaultSettings
{
    public static string DefaultStoragePath = "/files";
    public static int FolderCopyMaxNodeCount = 500;
    public static int ExpirationDateOfExternalShared = 7; //days

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

    public static class ParticipantIsolationFolder
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
}