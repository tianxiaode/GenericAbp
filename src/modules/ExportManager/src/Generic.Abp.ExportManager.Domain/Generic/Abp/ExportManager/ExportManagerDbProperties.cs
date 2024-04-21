namespace Generic.Abp.ExportManager
{
    public static class ExportManagerDbProperties
    {
        public static string DbTablePrefix { get; set; } = "ExportManager";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "ExportManager";
    }
}
