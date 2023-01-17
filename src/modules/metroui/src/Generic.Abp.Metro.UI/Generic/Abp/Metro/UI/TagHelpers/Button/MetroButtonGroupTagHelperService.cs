using Microsoft.AspNetCore.Razor.TagHelpers;
using NUglify.Helpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonGroupTagHelperService : MetroTagHelperService<MetroButtonGroupTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        AddClasses(context, output);
        AddAttributes(context, output);

        output.TagName = "div";
    }


    protected virtual void AddAttributes(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Multi)
        {
            output.Attributes.Add("data-mode", "multi");
        }
        output.Attributes.Add("data-role", "buttongroup");
    }
}
