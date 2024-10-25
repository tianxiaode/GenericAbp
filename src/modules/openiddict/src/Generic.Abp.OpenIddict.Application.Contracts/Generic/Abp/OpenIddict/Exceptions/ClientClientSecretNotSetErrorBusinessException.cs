using Volo.Abp;

namespace Generic.Abp.OpenIddict.Exceptions;

public class ClientClientSecretNotSetErrorBusinessException : BusinessException
{
    public ClientClientSecretNotSetErrorBusinessException()
    {
        Code = OpenIddictErrorCodes.ClientClientSecretNotSetError;
    }
}