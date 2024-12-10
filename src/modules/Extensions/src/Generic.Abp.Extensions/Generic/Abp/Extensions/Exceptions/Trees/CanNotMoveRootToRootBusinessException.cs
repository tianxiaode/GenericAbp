using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveRootToRootBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CanNotMoveRootToRoot)
{
}