namespace Generic.Abp.Extensions.Exceptions
{
    public class DuplicateWarningBusinessException : Volo.Abp.BusinessException
    {
        public DuplicateWarningBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.DuplicateWarning;
            WithData(BusinessExceptionErrorCodes.DuplicateWarningParamName, name)
                .WithData(BusinessExceptionErrorCodes.DuplicateWarningParamValue, value);
        }
    }
}