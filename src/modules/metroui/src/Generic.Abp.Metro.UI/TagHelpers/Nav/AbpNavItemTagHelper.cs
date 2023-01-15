namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavItemTagHelper : AbpTagHelper<AbpNavItemTagHelper, AbpNavItemTagHelperService>
{
    public bool? Dropdown { get; set; }

    public AbpNavItemTagHelper(AbpNavItemTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
