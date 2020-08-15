using System.Threading.Tasks;

namespace Generic.Abp.Themes.Toolbars
{
    public interface IToolbarManager
    {
        Task<Toolbar> GetAsync(string name);
    }
}
