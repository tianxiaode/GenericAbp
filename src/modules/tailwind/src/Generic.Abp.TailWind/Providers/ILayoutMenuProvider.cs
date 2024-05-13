namespace Generic.Abp.Tailwind.Providers;

public interface ILayoutMenuProvider
{
    Task<List<LayoutMenu>> GetMainMenusAsync();
    Task<List<LayoutMenu>> GetMobileMenusAsync();
}