using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class
    CausesCircularReferencesThatAreNotAllowedToBeMovedOrCopiedBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CausesCircularReferencesThatAreNotAllowedToBeMovedOrCopied);