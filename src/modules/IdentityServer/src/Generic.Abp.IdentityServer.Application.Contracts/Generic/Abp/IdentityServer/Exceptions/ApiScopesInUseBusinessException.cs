namespace Generic.Abp.IdentityServer.Exceptions
{
    public class ApiScopesInUseBusinessException: Volo.Abp.BusinessException
    {
        public ApiScopesInUseBusinessException() { 
            Code = BusinessExceptionErrorCodes.ApiScopesInUse;
            }
    }
}
