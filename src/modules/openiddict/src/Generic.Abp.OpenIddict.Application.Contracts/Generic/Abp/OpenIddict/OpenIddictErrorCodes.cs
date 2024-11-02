namespace Generic.Abp.OpenIddict
{
    public static class OpenIddictErrorCodes
    {
        //Add your business exception error codes here...
        public const string ClientTypeError = "Generic.Abp.OpenIddict.BusinessException:000001";
        public const string ParamValueName = "value";
        public const string ConsentTypeError = "Generic.Abp.OpenIddict.BusinessException:000002";
        public const string ApplicationTypeError = "Generic.Abp.OpenIddict.BusinessException:000003";
        public const string ClientClientSecretNotSetError = "Generic.Abp.OpenIddict.BusinessException:000004";
    }
}