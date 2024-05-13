using Generic.Abp.Tailwind.Views.Error;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.ExceptionHandling;

namespace Generic.Abp.Tailwind.Controllers;

public class ErrorController : AbpController
{
    private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
    private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
    private readonly IStringLocalizer<AbpUiResource> _localizer;
    private readonly TailwindErrorPageOptions _abpMetroErrorPageOptions;
    private readonly IExceptionNotifier _exceptionNotifier;
    private readonly AbpExceptionHandlingOptions _exceptionHandlingOptions;

    public ErrorController(
        IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
        IHttpExceptionStatusCodeFinder httpExceptionStatusCodeFinder,
        IOptions<TailwindErrorPageOptions> abpErrorPageOptions,
        IStringLocalizer<AbpUiResource> localizer,
        IExceptionNotifier exceptionNotifier,
        IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
    {
        _errorInfoConverter = exceptionToErrorInfoConverter;
        _statusCodeFinder = httpExceptionStatusCodeFinder;
        _localizer = localizer;
        _exceptionNotifier = exceptionNotifier;
        _exceptionHandlingOptions = exceptionHandlingOptions.Value;
        _abpMetroErrorPageOptions = abpErrorPageOptions.Value;
    }

    public async Task<IActionResult> Index(int httpStatusCode)
    {
        var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        var exception = exHandlerFeature != null
            ? exHandlerFeature.Error
            : new Exception(_localizer["UnhandledException"]);

        await _exceptionNotifier.NotifyAsync(new ExceptionNotificationContext(exception));

        var errorInfo = _errorInfoConverter.Convert(exception, options =>
        {
            options.SendExceptionsDetailsToClients = _exceptionHandlingOptions.SendExceptionsDetailsToClients;
            options.SendStackTraceToClients = _exceptionHandlingOptions.SendStackTraceToClients;
        });

        if (httpStatusCode == 0)
        {
            httpStatusCode = (int)_statusCodeFinder.GetStatusCode(HttpContext, exception);
        }

        HttpContext.Response.StatusCode = httpStatusCode;

        var page = GetErrorPageUrl(httpStatusCode);

        return View(page, new TailwindErrorViewModel
        {
            ErrorInfo = errorInfo,
            HttpStatusCode = httpStatusCode
        });
    }

    private string GetErrorPageUrl(int statusCode)
    {
        var page = _abpMetroErrorPageOptions.ErrorViewUrls.GetOrDefault(statusCode.ToString());

        if (string.IsNullOrWhiteSpace(page))
        {
            return "~/Views/Error/Default.cshtml";
        }

        return page;
    }
}