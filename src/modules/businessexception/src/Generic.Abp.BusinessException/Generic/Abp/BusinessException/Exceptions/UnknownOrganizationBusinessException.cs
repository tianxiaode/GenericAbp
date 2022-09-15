namespace Generic.Abp.BusinessException.Exceptions
{
    public class UnknownOrganizationBusinessException : Volo.Abp.BusinessException
    {
        public UnknownOrganizationBusinessException()
        {
            Code = BusinessExceptionErrorCodes.UnknownOrganization;
        }

    }
}
