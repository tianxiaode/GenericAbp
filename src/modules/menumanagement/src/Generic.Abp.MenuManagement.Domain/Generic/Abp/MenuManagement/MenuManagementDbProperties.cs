namespace Generic.Abp.MenuManagement
{
    public static class MenuManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "MenuManagement";
        public static string? DbSchema { get; set; } = null;

        public const string ConnectionStringName = "MenuManagement";
    }
}