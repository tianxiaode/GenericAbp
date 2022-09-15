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
    }
}
