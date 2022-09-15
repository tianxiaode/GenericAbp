namespace Generic.Abp.BusinessException.Exceptions;

public class EntityNotBeDeletedBusinessException: Volo.Abp.BusinessException
{
    public EntityNotBeDeletedBusinessException(string name, object value)
    {
        Code = BusinessExceptionErrorCodes.EntityNotBeDeleted;
        WithData(BusinessExceptionErrorCodes.EntityNotBeDeletedParamName, name)
            .WithData(BusinessExceptionErrorCodes.EntityNotBeDeletedParamValue, value);
    }

}