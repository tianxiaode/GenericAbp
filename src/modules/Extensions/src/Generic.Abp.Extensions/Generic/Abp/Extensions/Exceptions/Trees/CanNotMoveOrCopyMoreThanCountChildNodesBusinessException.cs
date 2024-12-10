using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveOrCopyMoreThanCountChildNodesBusinessException : BusinessException
{
    public CanNotMoveOrCopyMoreThanCountChildNodesBusinessException(int maxCount, long currentCount) : base(
        BusinessExceptionErrorCodes.CanNotMoveOrCopyMoreThanCountChildNodes)
    {
        WithData(BusinessExceptionErrorCodes.CanNotMoveOrCopyMoreThanCountChildNodesParamCount, maxCount)
            .WithData(BusinessExceptionErrorCodes.CanNotMoveOrCopyMoreThanCountChildNodesParamCurrent,
                currentCount);
    }
}