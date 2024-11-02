namespace Generic.Abp.Extensions.Exceptions
{
    public class ValueExceedsFieldLengthBusinessException : Volo.Abp.BusinessException
    {
        public ValueExceedsFieldLengthBusinessException(int length, object value)
        {
            Code = BusinessExceptionErrorCodes.ValueExceedsFieldLength;
            WithData(BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamLength, length)
                .WithData(BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamValue, value);
        }
    }
}