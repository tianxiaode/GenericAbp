using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions.Trees;

public class CanNotMoveOrCopyMoreThanCountNodesBusinessException : BusinessException
{
    public CanNotMoveOrCopyMoreThanCountNodesBusinessException(int maxCount, int currentCount) : base(
        BusinessExceptionErrorCodes
            .CanNotMoveOrCopyMoreThanCountNodes)
    {
        WithData(BusinessExceptionErrorCodes.CanNotMoveOrCopyMoreThanCountNodesParamCount, maxCount)
            .WithData(BusinessExceptionErrorCodes.CanNotMoveOrCopyMoreThanCountNodesParamCurrent, currentCount);
    }
}