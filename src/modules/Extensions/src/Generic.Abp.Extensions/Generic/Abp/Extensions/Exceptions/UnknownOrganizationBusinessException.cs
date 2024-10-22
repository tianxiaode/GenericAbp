namespace Generic.Abp.Extensions.Exceptions
{
    public class UnknownOrganizationBusinessException : Volo.Abp.BusinessException
    {
        public UnknownOrganizationBusinessException()
        {
            Code = BusinessExceptionErrorCodes.UnknownOrganization;
        }
    }
}