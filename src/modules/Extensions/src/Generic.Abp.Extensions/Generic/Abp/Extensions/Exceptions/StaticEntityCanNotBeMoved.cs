using Volo.Abp;

namespace Generic.Abp.Extensions.Exceptions;

public class StaticEntityCanNotBeMoved : BusinessException
{
    public StaticEntityCanNotBeMoved(string entityName, object value) : base(BusinessExceptionErrorCodes
        .StaticEntityCanNotBeMoved)
    {
        WithData(BusinessExceptionErrorCodes.StaticEntityCanNotBeMovedParamName, entityName)
            .WithData(BusinessExceptionErrorCodes.StaticEntityCanNotBeMovedParamValue, value);
    }
}