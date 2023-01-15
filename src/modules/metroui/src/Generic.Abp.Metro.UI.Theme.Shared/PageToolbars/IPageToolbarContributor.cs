using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Theme.Shared.PageToolbars;

public interface IPageToolbarContributor
{
    Task ContributeAsync(PageToolbarContributionContext context);
}
