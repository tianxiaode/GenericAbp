using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

[HtmlTargetElement(Attributes = "abp-navbar-brand")]
public class AbpNavbarBrandTagHelper : AbpTagHelper<AbpNavbarBrandTagHelper, AbpNavbarBrandTagHelperService>
{

    public AbpNavbarBrandTagHelper(AbpNavbarBrandTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
