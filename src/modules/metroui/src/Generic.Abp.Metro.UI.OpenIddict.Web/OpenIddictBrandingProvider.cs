using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Generic.Abp.Metro.UI.OpenIddict.Web;

[Dependency(ReplaceServices = true)]
public class OpenIddictBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OpenIddict";
}