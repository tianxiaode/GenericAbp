using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ProxyScripting;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Http.ProxyScripting;
using Volo.Abp.Minify.Scripts;

namespace Generic.Abp.TailWindCss.Account.Web.Controllers;

[Area("TailWind")]
[Route("tailwind/ServiceProxyScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class ServiceProxyScriptController : AbpController
{
    private readonly IProxyScriptManager _proxyScriptManager;
    private readonly AbpAspNetCoreMvcOptions _options;
    private readonly IJavascriptMinifier _javascriptMinifier;

    public ServiceProxyScriptController(IProxyScriptManager proxyScriptManager,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier)
    {
        _proxyScriptManager = proxyScriptManager;
        _options = options.Value;
        _javascriptMinifier = javascriptMinifier;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public ActionResult GetAll(ServiceProxyGenerationModel model)
    {
        model.Normalize();

        var script = _proxyScriptManager.GetScript(model.CreateOptions());
        return Content(
            _options.MinifyGeneratedScript == true
                ? _javascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }
}