using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownItemTagHelperService : MetroTagHelperService<MetroDropdownItemTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;

        await SetActiveClassIfActiveAsync(context, output);
        await SetDisabledClassIfDisabledAsync(context, output);
    }

    protected virtual Task SetActiveClassIfActiveAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Active ?? false)
        {
            output.Attributes.AddClass("active");
        }
        return Task.CompletedTask;
    }

    protected virtual Task SetDisabledClassIfDisabledAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Disabled ?? false)
        {
            output.Attributes.AddClass("disabled");
        }
        return Task.CompletedTask;
    }
}
