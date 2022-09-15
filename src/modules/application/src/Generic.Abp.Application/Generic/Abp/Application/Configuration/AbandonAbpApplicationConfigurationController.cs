using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Application.Configuration;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpApplicationConfigurationController), IncludeSelf = true)]
[RemoteService(false)]
public class AbandonAbpApplicationConfigurationController : AbpApplicationConfigurationController
{
    public AbandonAbpApplicationConfigurationController(
        IAbpApplicationConfigurationAppService applicationConfigurationAppService,
        IAbpAntiForgeryManager antiForgeryManager) : base(applicationConfigurationAppService, antiForgeryManager)
    {
    }
}