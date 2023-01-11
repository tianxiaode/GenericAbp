using Generic.Abp.PhoneLogin.IdentityServer.AspNetIdentity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.AspNetIdentity;

namespace Generic.Abp.PhoneLogin.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(GenericAbpPhoneLoginDomainModule)
    )]

    public class GenericAbpPhoneLoginIdentityServerDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IIdentityServerBuilder>(builder =>
            {
                builder.AddResourceOwnerValidator<PhoneLoginResourceOwnerPasswordValidator>();
            });
            context.Services.Replace(
              ServiceDescriptor.Transient<AbpResourceOwnerPasswordValidator, PhoneLoginResourceOwnerPasswordValidator>());
        }
    }
}
