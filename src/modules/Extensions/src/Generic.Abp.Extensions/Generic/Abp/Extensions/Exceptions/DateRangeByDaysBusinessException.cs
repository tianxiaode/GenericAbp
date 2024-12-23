﻿namespace Generic.Abp.Extensions.Exceptions
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