using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class AbpDropdownMenuTagHelperService : AbpTagHelperService<AbpDropdownMenuTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("dropdown-menu");
        output.TagMode = TagMode.StartTagAndEndTag;

        SetAlign(context, output);
    }

    protected virtual void SetAlign(TagHelperContext context, TagHelperOutput output)
    {
        switch (TagHelper.Align)
        {
            case DropdownAlign.End:
                output.Attributes.AddClass("dropdown-menu-end");
                return;
            case DropdownAlign.Start:
                return;
        }
    }
}
