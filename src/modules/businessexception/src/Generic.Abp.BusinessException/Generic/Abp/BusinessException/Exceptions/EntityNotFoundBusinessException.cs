namespace Generic.Abp.BusinessException.Exceptions
{
    public class EntityNotFoundBusinessException : Volo.Abp.BusinessException
    {
        public EntityNotFoundBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.EntityNotFound;
            WithData(BusinessExceptionErrorCodes.EntityNotFoundParamName, name)
                .WithData(BusinessExceptionErrorCodes.EntityNotFoundParamValue, value);
        }

    }
}
