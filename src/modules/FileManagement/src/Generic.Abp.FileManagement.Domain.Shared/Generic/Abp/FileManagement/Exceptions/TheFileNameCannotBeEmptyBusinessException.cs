using System;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.FileManagement.Exceptions;

public class TheFileNameCannotBeEmptyBusinessException: Volo.Abp.BusinessException
{
    public TheFileNameCannotBeEmptyBusinessException() 
    {
        Code = FileManagementErrorCodes.TheFileNameCannotBeEmpty;
    }
}