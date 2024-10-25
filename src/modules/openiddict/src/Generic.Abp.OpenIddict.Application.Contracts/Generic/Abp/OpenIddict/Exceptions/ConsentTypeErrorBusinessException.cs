namespace Generic.Abp.OpenIddict.Exceptions
{
    public class ConsentTypeErrorBusinessException : Volo.Abp.BusinessException
    {
        public ConsentTypeErrorBusinessException(string value)
        {
            Code = OpenIddictErrorCodes.ConsentTypeError;
            WithData(OpenIddictErrorCodes.ParamValueName, value);
        }
    }
}