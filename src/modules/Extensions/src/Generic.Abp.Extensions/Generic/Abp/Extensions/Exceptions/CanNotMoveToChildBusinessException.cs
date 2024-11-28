using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class CanNotMoveToChildBusinessException() : BusinessException(BusinessExceptionErrorCodes.CanNotMoveToChild)
{
}