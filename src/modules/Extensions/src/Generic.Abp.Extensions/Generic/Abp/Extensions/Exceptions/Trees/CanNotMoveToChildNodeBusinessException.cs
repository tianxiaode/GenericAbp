using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveToChildNodeBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CanNotMoveToChildNode)
{
}