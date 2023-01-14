using Generic.Abp.PhoneLogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Generic.Abp.PhoneLogin
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpPhoneLoginDomainSharedModule)
    )]
    public class GenericAbpPhoneLoginDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IdentityBuilder>(builder =>
            {
                builder.AddUserValidator<PhoneLoginUserValidator>();
            });
        }

    }
}
