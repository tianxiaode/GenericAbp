using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Minify.Scripts;

namespace Generic.Abp.Tailwind.Controllers;

[Area("Tailwind")]
[Route("Tailwind/ApplicationLocalizationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class ApplicationLocalizationScriptController : AbpController
{
    protected AbpApplicationLocalizationAppService LocalizationAppService { get; }
    protected AbpAspNetCoreMvcOptions Options { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IJavascriptMinifier JavascriptMinifier { get; }

    public ApplicationLocalizationScriptController(
        AbpApplicationLocalizationAppService localizationAppService,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJsonSerializer jsonSerializer,
        IJavascriptMinifier javascriptMinifier)
    {
        LocalizationAppService = localizationAppService;
        JsonSerializer = jsonSerializer;
        JavascriptMinifier = javascriptMinifier;
        Options = options.Value;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public async Task<ActionResult> GetAsync(ApplicationLocalizationRequestDto input)
    {
        var script = CreateScript(
            await LocalizationAppService.GetAsync(input)
        );

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    private string CreateScript(ApplicationLocalizationDto localizationDto)
    {
        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine(
            $" abp.localization = _.defaultsDeep( abp.localization, {JsonSerializer.Serialize(localizationDto, indented: true)})");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}