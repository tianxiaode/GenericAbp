namespace Generic.Abp.PhoneLogin
{
    public static class PhoneLoginDbProperties
    {
        public static string DbTablePrefix { get; set; } = "PhoneLogin";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "PhoneLogin";
    }
}
