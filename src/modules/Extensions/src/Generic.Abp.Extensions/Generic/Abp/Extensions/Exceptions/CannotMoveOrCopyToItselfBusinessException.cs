using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class CannotMoveOrCopyToItselfBusinessException : BusinessException
{
    public CannotMoveOrCopyToItselfBusinessException()
    {
        Code = BusinessExceptionErrorCodes.CannotMoveOrCopyToItself;
    }
}