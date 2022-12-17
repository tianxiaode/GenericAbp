namespace Generic.Abp.IdentityServer.Exceptions
{
    public class SecretTypeErrorBusinessException: Volo.Abp.BusinessException
    {
        public SecretTypeErrorBusinessException()
        {
            Code = BusinessExceptionErrorCodes.SecretTypeError;

        }

    }
}
