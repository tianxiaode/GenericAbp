using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonGroupTagHelperService : MetroTagHelperService<MetroButtonGroupTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await AddAttributesAsync(context, output);

        output.TagName = "div";
    }


    protected virtual Task AddAttributesAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Multi)
        {
            output.Attributes.Add("data-mode", "multi");
        }
        output.Attributes.Add("data-role", "buttongroup");
        return Task.CompletedTask;
    }
}
