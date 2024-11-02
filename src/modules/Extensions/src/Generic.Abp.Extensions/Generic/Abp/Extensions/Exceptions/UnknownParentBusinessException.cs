namespace Generic.Abp.Extensions.Exceptions;

public class UnknownParentBusinessException : Volo.Abp.BusinessException
{
    public UnknownParentBusinessException()
    {
        Code = BusinessExceptionErrorCodes.UnknownParent;
    }
}