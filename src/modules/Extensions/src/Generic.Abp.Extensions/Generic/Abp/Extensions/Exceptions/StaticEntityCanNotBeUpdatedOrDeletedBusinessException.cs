using System;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class StaticEntityCanNotBeUpdatedOrDeletedBusinessException : BusinessException
{
    public StaticEntityCanNotBeUpdatedOrDeletedBusinessException(string name, object value)
    {
        Code = BusinessExceptionErrorCodes.StaticEntityCanNotBeUpdatedOrDeleted;
        WithData(BusinessExceptionErrorCodes.StaticEntityCanNotBeUpdatedOrDeletedParamName, name)
            .WithData(BusinessExceptionErrorCodes.StaticEntityCanNotBeUpdatedOrDeletedParamValue, value);
    }
}