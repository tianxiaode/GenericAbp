namespace Generic.Abp.BusinessException.Exceptions
{
    public class DateRangeByMonthBusinessException : Volo.Abp.BusinessException
    {
        public DateRangeByMonthBusinessException(int value)
        {
            Code = BusinessExceptionErrorCodes.DateRangeByMonth;
            WithData(BusinessExceptionErrorCodes.DateRangeByMonthValue, value);
        }

    }
}
