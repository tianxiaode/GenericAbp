namespace Generic.Abp.Extensions.Exceptions
{
    public class NoPermissionAccessBusinessException : Volo.Abp.BusinessException
    {
        public NoPermissionAccessBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.NoPermissionAccess;
            WithData(BusinessExceptionErrorCodes.NoPermissionParamName, name)
                .WithData(BusinessExceptionErrorCodes.NoPermissionParamValue, value);
        }
    }
}