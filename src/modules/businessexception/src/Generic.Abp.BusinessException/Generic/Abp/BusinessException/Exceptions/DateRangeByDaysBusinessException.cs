namespace Generic.Abp.BusinessException.Exceptions
{
    public class DateRangeByDaysBusinessException : Volo.Abp.BusinessException
    {
        public DateRangeByDaysBusinessException(int value)
        {
            Code = BusinessExceptionErrorCodes.DateRangeByDays;
            WithData(BusinessExceptionErrorCodes.DateRangeByDaysValue, value);
        }

    }
}
