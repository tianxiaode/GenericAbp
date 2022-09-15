using System.Threading.Tasks;

namespace Generic.Abp.Themes.Shared.PageToolbars
{
    public abstract class PageToolbarContributor : IPageToolbarContributor
    {
        public abstract Task ContributeAsync(PageToolbarContributionContext context);
    }
}
