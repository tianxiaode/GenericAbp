using System.Threading.Tasks;

namespace Generic.Abp.Themes.Toolbars
{
    public interface IToolbarContributor
    {
        Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
    }
}
