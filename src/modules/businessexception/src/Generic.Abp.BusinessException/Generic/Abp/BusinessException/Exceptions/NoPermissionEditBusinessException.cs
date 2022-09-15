namespace Generic.Abp.BusinessException.Exceptions
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
