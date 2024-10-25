namespace Generic.Abp.OpenIddict.Exceptions
{
    public class ClientTypeErrorBusinessException : Volo.Abp.BusinessException
    {
        public ClientTypeErrorBusinessException(string value)
        {
            Code = OpenIddictErrorCodes.ClientTypeError;
            WithData(OpenIddictErrorCodes.ParamValueName, value);
        }
    }
}