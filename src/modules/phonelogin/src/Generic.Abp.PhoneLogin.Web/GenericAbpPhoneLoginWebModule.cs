using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.PhoneLogin.Web;

[DependsOn(
    typeof(AbpIdentityWebModule)
    )]
public class GenericAbpPhoneLoginWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpPhoneLoginWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpPhoneLoginWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpPhoneLoginWebModule>();
        Configure<AbpAutoMapperOptions>(
            options =>
            {
                options.AddMaps<GenericAbpPhoneLoginWebModule>(validate: true);
            });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.

        });
    }

}
