using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Theme.Shared.PageToolbars;

public abstract class PageToolbarContributor : IPageToolbarContributor
{
    public abstract Task ContributeAsync(PageToolbarContributionContext context);
}
