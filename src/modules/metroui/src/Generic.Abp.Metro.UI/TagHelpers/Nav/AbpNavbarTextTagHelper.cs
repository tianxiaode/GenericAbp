using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

[HtmlTargetElement("span", Attributes = "abp-navbar-text")]
public class AbpNavbarTextTagHelper : AbpTagHelper<AbpNavbarTextTagHelper, AbpNavbarTextTagHelperService>
{
    public AbpNavbarTextTagHelper(AbpNavbarTextTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
