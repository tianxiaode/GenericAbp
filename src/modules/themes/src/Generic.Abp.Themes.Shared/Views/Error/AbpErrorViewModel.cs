using Volo.Abp.Http;

namespace Generic.Abp.Themes.Shared.Views.Error
{
    public class AbpErrorViewModel
    {
        public RemoteServiceErrorInfo ErrorInfo { get; set; }

        public int HttpStatusCode { get; set; }
    }
}
