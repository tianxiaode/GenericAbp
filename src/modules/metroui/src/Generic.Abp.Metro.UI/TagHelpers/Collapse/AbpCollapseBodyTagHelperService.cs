using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Collapse;

public class AbpCollapseBodyTagHelperService : AbpTagHelperService<AbpCollapseBodyTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("collapse");
        output.Attributes.Add("id", TagHelper.Id);

        if (TagHelper.Show ?? false)
        {
            output.Attributes.AddClass("show");
        }

        if (TagHelper.Multi ?? false)
        {
            output.Attributes.AddClass("multi-collapse");
        }

        var childContent = await output.GetChildContentAsync();

        output.Content.SetHtmlContent(childContent);
    }
}
