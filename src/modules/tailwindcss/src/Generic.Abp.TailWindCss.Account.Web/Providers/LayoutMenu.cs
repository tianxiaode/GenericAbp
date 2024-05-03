namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public class LayoutMenu(string name, string href, string[] permissions = null) : ILayoutMenu
{
    public string Name { get; set; } = name;
    public string Href { get; set; } = href;

    public string[] Permissions { get; set; } = permissions;
}