using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveParentAndItsChildrenBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CanNotMoveParentAndItsChildren)
{
}