using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Theme.Shared.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
