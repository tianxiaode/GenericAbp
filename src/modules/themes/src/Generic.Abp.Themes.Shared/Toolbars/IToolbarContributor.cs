using System.Threading.Tasks;

namespace Generic.Abp.Themes.Shared.Toolbars
{
    public interface IToolbarContributor
    {
        Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
    }
}
