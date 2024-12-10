using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class NoSelectedItemFoundBusinessException() : BusinessException(BusinessExceptionErrorCodes.NoSelectedItemFound)
{
}