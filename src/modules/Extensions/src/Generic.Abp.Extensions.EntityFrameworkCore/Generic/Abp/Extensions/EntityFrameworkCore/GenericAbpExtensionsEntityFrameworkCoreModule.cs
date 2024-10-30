using Generic.Abp.Extensions.EntityFrameworkCore.SecurityLogs;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.Extensions.EntityFrameworkCore;

[DependsOn(
    typeof(AbpIdentityEntityFrameworkCoreModule)
)]
public class GenericAbpExtensionsEntityFrameworkCoreModule : AbpModule
{
}