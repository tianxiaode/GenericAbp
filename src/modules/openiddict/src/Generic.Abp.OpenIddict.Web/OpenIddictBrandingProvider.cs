using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.OpenIddict.Web;

[Dependency(ReplaceServices = true)]
public class OpenIddictBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OpenIddict";
}
