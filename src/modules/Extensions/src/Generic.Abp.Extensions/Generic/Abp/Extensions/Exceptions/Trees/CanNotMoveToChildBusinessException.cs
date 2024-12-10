using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveToChildBusinessException() : BusinessException(BusinessExceptionErrorCodes.CanNotMoveToChild)
{
}