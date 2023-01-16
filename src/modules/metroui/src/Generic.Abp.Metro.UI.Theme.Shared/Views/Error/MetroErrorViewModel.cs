using Volo.Abp.Http;

namespace Generic.Abp.Metro.UI.Theme.Shared.Views.Error;

public class MetroErrorViewModel
{

    public RemoteServiceErrorInfo ErrorInfo { get; set; } = default!;

    public int HttpStatusCode { get; set; }
}
