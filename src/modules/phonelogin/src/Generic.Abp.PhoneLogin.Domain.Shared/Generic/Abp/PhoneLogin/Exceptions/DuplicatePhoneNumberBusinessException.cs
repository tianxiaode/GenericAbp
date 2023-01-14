using Volo.Abp;

namespace Generic.Abp.PhoneLogin.Exceptions;

public class DuplicatePhoneNumberBusinessException : BusinessException
{
    public DuplicatePhoneNumberBusinessException(object value)
    {
        Code = PhoneLoginErrorCodes.DuplicatePhoneNumber;
        WithData(PhoneLoginErrorCodes.DuplicatePhoneNumberParamPhone, value);
    }
}