namespace Generic.Abp.OpenIddict.Exceptions
{
    public class ClientTypeErrorBusinessException : Volo.Abp.BusinessException
    {
        public ClientTypeErrorBusinessException()
        {
            Code = OpenIddictErrorCodes.ClientTypeError;
        }
    }
}
