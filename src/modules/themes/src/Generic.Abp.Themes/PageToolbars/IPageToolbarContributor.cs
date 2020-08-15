using System.Threading.Tasks;

namespace Generic.Abp.Themes.PageToolbars
{
    public interface IPageToolbarContributor
    {
        Task ContributeAsync(PageToolbarContributionContext context);
    }
}
