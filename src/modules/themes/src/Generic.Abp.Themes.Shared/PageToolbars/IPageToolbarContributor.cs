using System.Threading.Tasks;

namespace Generic.Abp.Themes.Shared.PageToolbars
{
    public interface IPageToolbarContributor
    {
        Task ContributeAsync(PageToolbarContributionContext context);
    }
}
