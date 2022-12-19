namespace Generic.Abp.IdentityServer.Exceptions
{
    public class ProtocolTypeErrorBusinessException : Volo.Abp.BusinessException
    {
        public ProtocolTypeErrorBusinessException()
        {
            Code = BusinessExceptionErrorCodes.ProtocolTypeError;

        }

    }
}
