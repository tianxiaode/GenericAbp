using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

[HtmlTargetElement(Attributes = "abp-nav-link")]
public class AbpNavLinkTagHelper : AbpTagHelper<AbpNavLinkTagHelper, AbpNavLinkTagHelperService>
{
    public bool? Active { get; set; }

    public bool? Disabled { get; set; }

    public AbpNavLinkTagHelper(AbpNavLinkTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
