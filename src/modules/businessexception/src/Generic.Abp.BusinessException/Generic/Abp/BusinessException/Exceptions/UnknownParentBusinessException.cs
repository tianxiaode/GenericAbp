namespace Generic.Abp.BusinessException.Exceptions;

public class UnknownParentBusinessException : Volo.Abp.BusinessException
{
    public UnknownParentBusinessException()
    {
        Code = BusinessExceptionErrorCodes.UnknownParent;
    }
}