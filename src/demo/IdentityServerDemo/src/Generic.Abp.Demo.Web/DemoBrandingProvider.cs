using Generic.Abp.Demo.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Generic.Abp.Demo.Web
{
    [Dependency(ReplaceServices = true)]
    public class DemoBrandingProvider : DefaultBrandingProvider
    {
        private readonly IStringLocalizer<DemoResource> _localizer;
        public DemoBrandingProvider(IStringLocalizer<DemoResource> localizer)
        {
            _localizer = localizer;
        }

        public override string AppName => _localizer["AppName"];
    }
}
