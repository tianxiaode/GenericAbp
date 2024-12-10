using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class
    CausesCircularReferencesThatAreNotAllowedToBeMovedOrCopiedBusinessException()
    : BusinessException(BusinessExceptionErrorCodes.CausesCircularReferencesThatAreNotAllowedToBeMovedOrCopied);