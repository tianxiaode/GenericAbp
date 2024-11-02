namespace Generic.Abp.Extensions.Exceptions
{
    public class NoPermissionEditBusinessException : Volo.Abp.BusinessException
    {
        public NoPermissionEditBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.NoPermissionEdit;
            WithData(BusinessExceptionErrorCodes.NoPermissionParamName, name)
                .WithData(BusinessExceptionErrorCodes.NoPermissionParamValue, value);
        }
    }
}