namespace Generic.Abp.OpenIddict.Exceptions
{
    public class ConsentTypeErrorBusinessException : Volo.Abp.BusinessException
    {
        public ConsentTypeErrorBusinessException()
        {
            Code = OpenIddictErrorCodes.ConsentTypeError;
        }
    }
}
