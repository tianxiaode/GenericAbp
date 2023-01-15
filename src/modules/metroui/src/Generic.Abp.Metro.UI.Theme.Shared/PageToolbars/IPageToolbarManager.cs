using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Theme.Shared.PageToolbars;

public interface IPageToolbarManager
{
    Task<PageToolbarItem[]> GetItemsAsync(string pageName);
}
