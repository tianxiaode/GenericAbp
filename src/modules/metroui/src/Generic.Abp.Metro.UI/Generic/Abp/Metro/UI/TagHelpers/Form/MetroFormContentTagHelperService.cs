using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelperService : MetroTagHelperService<MetroFormContentTagHelper>
{
    public MetroFormContentTagHelperService()
    {
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        context.Items["FormContent"] = new FormContent(TagHelper.Cols, TagHelper.Horizontal,TagHelper.LabelWidth);
        output.TagName = "div";
        output.Attributes.AddClass("d-flex");
        output.Attributes.AddClass("flex-wrap");
        return Task.CompletedTask;
    }
}
