namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavbarToggleTagHelper : AbpTagHelper<AbpNavbarToggleTagHelper, AbpNavbarToggleTagHelperService>
{
    public string Id { get; set; }

    public AbpNavbarToggleTagHelper(AbpNavbarToggleTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
