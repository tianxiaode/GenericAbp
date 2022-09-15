namespace Generic.Abp.BusinessException.Exceptions
{
    public class HasChildrenCanNotDeletedBusinessException : Volo.Abp.BusinessException
    {
        public HasChildrenCanNotDeletedBusinessException(string name, object value)
        {
            Code = BusinessExceptionErrorCodes.HasChildrenCanNotDeleted;
            WithData(BusinessExceptionErrorCodes.HasChildrenCanNotDeletedParamName, name)
                .WithData(BusinessExceptionErrorCodes.HasChildrenCanNotDeletedParamValue, value);
        }

    }
}
