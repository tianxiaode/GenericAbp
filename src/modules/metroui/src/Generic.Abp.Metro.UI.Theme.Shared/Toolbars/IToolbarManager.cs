using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Theme.Shared.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
