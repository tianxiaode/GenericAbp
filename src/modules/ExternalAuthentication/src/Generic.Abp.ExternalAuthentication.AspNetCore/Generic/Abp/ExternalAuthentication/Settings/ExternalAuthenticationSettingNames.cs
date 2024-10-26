namespace Generic.Abp.ExternalAuthentication.Settings;

public static class ExternalAuthenticationSettingNames
{
    private const string Prefix = "Generic.Abp.ExternalAuthentication";
    public const string EnableLocalLogin = "Abp.Account.EnableLocalLogin";

    public static class Provider
    {
        public const string ProviderPrefix = Prefix + ".Provider.";
    }

    public static class NewUser
    {
        public const string NewUserPrefix = Prefix + ".NewUserPrefix";
        public const string NewUserEmailSuffix = Prefix + ".NewUserEmail";
    }
}