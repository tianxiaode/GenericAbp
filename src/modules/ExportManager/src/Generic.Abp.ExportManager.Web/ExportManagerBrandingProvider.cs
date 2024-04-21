using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.ExportManager.Web;

[Dependency(ReplaceServices = true)]
public class ExportManagerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ExportManager";
}
