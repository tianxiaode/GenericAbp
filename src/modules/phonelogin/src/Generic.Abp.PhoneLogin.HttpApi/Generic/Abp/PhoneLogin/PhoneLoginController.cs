using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.PhoneLogin
{
    public abstract class PhoneLoginController : AbpController
    {
        protected PhoneLoginController()
        {
            LocalizationResource = typeof(PhoneLoginResource);
        }
    }
}
