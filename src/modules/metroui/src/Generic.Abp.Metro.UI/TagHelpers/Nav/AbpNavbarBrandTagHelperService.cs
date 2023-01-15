using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavbarBrandTagHelperService : AbpTagHelperService<AbpNavbarBrandTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("abp-navbar-brand");
        output.Attributes.AddClass("navbar-brand");
    }
}
