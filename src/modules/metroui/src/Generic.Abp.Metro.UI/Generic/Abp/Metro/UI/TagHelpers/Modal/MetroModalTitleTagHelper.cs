using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

[HtmlTargetElement("metro-modal-title", TagStructure = TagStructure.WithoutEndTag)]
public class MetroModalTitleTagHelper : TagHelper
{
    public string Title { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("dialog-title");
        output.Content.AppendHtml(Title);
        return Task.CompletedTask;
    }
}