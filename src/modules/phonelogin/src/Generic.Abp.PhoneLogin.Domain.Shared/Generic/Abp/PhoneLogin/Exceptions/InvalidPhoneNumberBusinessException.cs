using Volo.Abp;

namespace Generic.Abp.PhoneLogin.Exceptions;

public class InvalidPhoneNumberBusinessException : BusinessException
{
    public InvalidPhoneNumberBusinessException(object value)
    {
        Code = PhoneLoginErrorCodes.InvalidPhoneNumber;
        WithData(PhoneLoginErrorCodes.InvalidPhoneNumberParamPhone, value);
    }

}