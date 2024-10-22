namespace Generic.Abp.Extensions.Exceptions
{
    public class NoPermissionDeleteBusinessException : Volo.Abp.BusinessException
    {
        public NoPermissionDeleteBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.NoPermissionDelete;
            WithData(BusinessExceptionErrorCodes.NoPermissionParamName, name)
                .WithData(BusinessExceptionErrorCodes.NoPermissionParamValue, value);
        }
    }
}