namespace Generic.Abp.FileManagement
{
    public static class FileManagementErrorCodes
    {
        //Add your business exception error codes here...
        public const string TheFileNameCannotBeEmpty = "Generic.Abp.FileManagement:000001";

        public const string FileChunkError = "Generic.Abp.FileManagement:000002";

        public const string InvalidFileType = "Generic.Abp.FileManagement:000003";

        public const string FileSizeOutOfRange = "Generic.Abp.FileManagement:000004";

        public const string FileSizeOutOfRangeValue = "{Value}";
        public const string FileSizeOutOfRangeMax = "{Max}";

        public const string InsufficientStorageSpaceBusinessException = "Generic.Abp.FileManagement:000005";
        public const string InsufficientStorageSpaceValue = "{Value}";
        public const string InsufficientStorageSpaceUsed = "{Used}";
        public const string InsufficientStorageSpaceMax = "{Max}";

        public const string StaticFolderCanNotBeMoveOrDeleted = "Generic.Abp.FileManagement:000006";
        public const string OnlyMovePublicFolder = "Generic.Abp.FileManagement:000007";

        public const string OnlyMoveMaxFilesAndFoldersInOnTime = "Generic.Abp.FileManagement:000008";
        public const string OnlyMoveMaxFilesAndFoldersInOnTimeMax = "{Max}";
        public const string OnlyMoveMaxFilesAndFoldersInOnTimeCount = "{Count}";
    }
}