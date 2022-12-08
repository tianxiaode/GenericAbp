namespace Generic.Abp.IdentityServer.Exceptions
{
    public class RemovingTheDefaultScopeIsNotAllowedBusinessException: Volo.Abp.BusinessException
    {
        public RemovingTheDefaultScopeIsNotAllowedBusinessException() { 
            Code = BusinessExceptionErrorCodes.RemovingTheDefaultScopeIsNotAllowed;
            
            }
    }
}
