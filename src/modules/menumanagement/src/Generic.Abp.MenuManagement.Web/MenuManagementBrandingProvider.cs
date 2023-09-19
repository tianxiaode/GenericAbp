using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.MenuManagement.Web;

[Dependency(ReplaceServices = true)]
public class MenuManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MenuManagement";
}
