using System.Threading.Tasks;

namespace Generic.Abp.Themes.Shared.Toolbars
{
    public interface IToolbarManager
    {
        Task<Toolbar> GetAsync(string name);
    }
}
