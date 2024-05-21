using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.OAuthProviderManager.Web;

[Dependency(ReplaceServices = true)]
public class OAuthProviderManagerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OAuthProviderManager";
}
