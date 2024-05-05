using Volo.Abp.Http;

namespace Generic.Abp.TailWindCss.Account.Web.Views.Error;

public class TailWindErrorViewModel
{
    public RemoteServiceErrorInfo ErrorInfo { get; set; } = default!;

    public int HttpStatusCode { get; set; }
}