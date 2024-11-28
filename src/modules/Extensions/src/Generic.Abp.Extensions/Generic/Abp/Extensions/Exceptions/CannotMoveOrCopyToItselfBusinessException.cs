using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class CanNotMoveOrCopyToItselfBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CanNotMoveOrCopyToItself);