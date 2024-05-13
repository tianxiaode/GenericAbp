using Volo.Abp.Http;

namespace Generic.Abp.Tailwind.Views.Error;

public class TailwindErrorViewModel
{
    public RemoteServiceErrorInfo ErrorInfo { get; set; } = default!;

    public int HttpStatusCode { get; set; }
}