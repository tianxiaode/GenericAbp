using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.OpenIddict.Exceptions;

public class ApplicationTypeErrorBusinessException : Volo.Abp.BusinessException
{
    public ApplicationTypeErrorBusinessException(string value)
    {
        Code = OpenIddictErrorCodes.ApplicationTypeError;
        WithData(OpenIddictErrorCodes.ParamValueName, value);
    }
}