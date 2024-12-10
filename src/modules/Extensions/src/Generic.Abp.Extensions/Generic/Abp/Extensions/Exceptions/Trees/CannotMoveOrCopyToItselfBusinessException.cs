using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveOrCopyToItselfBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CanNotMoveOrCopyToItself);