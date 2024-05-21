namespace Generic.Abp.OAuthProviderManager
{
    public static class OAuthProviderManagerDbProperties
    {
        public static string DbTablePrefix { get; set; } = "OAuthProviderManager";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "OAuthProviderManager";
    }
}
