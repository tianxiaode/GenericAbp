using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.PhoneLogin
{
    public abstract class PhoneLoginAppService : ApplicationService
    {
        protected PhoneLoginAppService()
        {
            LocalizationResource = typeof(PhoneLoginResource);
            ObjectMapperContext = typeof(GenericAbpPhoneLoginApplicationModule);
        }
    }
}
