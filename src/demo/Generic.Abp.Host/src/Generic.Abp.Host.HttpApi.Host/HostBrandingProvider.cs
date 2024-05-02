using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Generic.Abp.Host;

[Dependency(ReplaceServices = true)]
public class HostBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Host";
}
