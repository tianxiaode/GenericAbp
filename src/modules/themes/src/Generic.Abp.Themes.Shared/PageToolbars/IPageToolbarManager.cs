using System.Threading.Tasks;

namespace Generic.Abp.Themes.Shared.PageToolbars
{
    public interface IPageToolbarManager
    {
        Task<PageToolbarItem[]> GetItemsAsync(string pageName);
    }
}
