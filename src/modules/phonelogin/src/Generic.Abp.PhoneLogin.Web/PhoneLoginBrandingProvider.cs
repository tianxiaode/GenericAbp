using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.PhoneLogin.Web;

[Dependency(ReplaceServices = true)]
public class PhoneLoginBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "PhoneLogin";
}
