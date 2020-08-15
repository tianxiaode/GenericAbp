using System.Threading.Tasks;

namespace Generic.Abp.Themes.PageToolbars
{
    public interface IPageToolbarManager
    {
        Task<PageToolbarItem[]> GetItemsAsync(string pageName);
    }
}
