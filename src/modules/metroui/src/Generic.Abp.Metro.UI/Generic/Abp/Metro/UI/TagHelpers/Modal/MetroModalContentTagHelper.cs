using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

public class MetroModalContentTagHelper : TagHelper
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var attributes = output.Attributes;
        attributes.AddClass("dialog-content");
        attributes.AddClass("flex-fill");
        attributes.AddClass("scroll-y");
        return Task.CompletedTask;
    }
}