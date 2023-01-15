using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavbarTextTagHelperService : AbpTagHelperService<AbpNavbarTextTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("navbar-text");
        output.Attributes.RemoveAll("abp-navbar-text");
    }
}
