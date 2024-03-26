using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Auditing;

namespace Generic.Abp.Application.Configuration;

[Area("Generic")]
[RemoteService(Name = "Application")]
[DisableAuditing]
[ControllerName("Configuration")]
[Route("api")]
public class GenericAbpApplicationConfigurationController : AbpControllerBase,
    IGenericAbpApplicationConfigurationAppService
{
    private readonly IGenericAbpApplicationConfigurationAppService _genericAbpApplicationConfigurationAppService;
    private readonly IAbpAntiForgeryManager _abpAntiForgeryManager;

    public GenericAbpApplicationConfigurationController(
        IGenericAbpApplicationConfigurationAppService genericAbpApplicationConfigurationAppService,
        IAbpAntiForgeryManager abpAntiForgeryManager)
    {
        _genericAbpApplicationConfigurationAppService = genericAbpApplicationConfigurationAppService;
        _abpAntiForgeryManager = abpAntiForgeryManager;
    }

    [HttpGet]
    [Route("localization")]
    public virtual Task<ApplicationLocalizationConfigurationDto> GetLocalizationAsync()
    {
        return _genericAbpApplicationConfigurationAppService.GetLocalizationAsync();
    }
}