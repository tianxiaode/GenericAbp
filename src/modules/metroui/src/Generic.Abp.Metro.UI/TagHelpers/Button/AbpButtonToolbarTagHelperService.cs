using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class AbpButtonToolbarTagHelperService : AbpTagHelperService<AbpButtonToolbarTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("btn-toolbar");
        output.Attributes.Add("role", "toolbar");
    }
}
