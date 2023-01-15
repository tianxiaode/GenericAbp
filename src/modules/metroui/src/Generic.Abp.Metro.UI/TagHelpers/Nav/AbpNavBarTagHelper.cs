namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavBarTagHelper : AbpTagHelper<AbpNavBarTagHelper, AbpNavBarTagHelperService>
{
    public AbpNavbarSize Size { get; set; } = AbpNavbarSize.Default;

    public AbpNavbarStyle NavbarStyle { get; set; } = AbpNavbarStyle.Default;

    public AbpNavBarTagHelper(AbpNavBarTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
