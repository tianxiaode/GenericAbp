namespace Generic.Abp.FileManagement
{
    public static class FileManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "FileManagement";

        public static string? DbSchema { get; set; } = default!;

        public const string ConnectionStringName = "FileManagement";
    }
}