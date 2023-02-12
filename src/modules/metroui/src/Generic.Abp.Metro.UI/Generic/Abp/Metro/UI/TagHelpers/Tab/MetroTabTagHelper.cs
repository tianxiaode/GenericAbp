using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Tab;

[HtmlTargetElement("metro-tab", TagStructure = TagStructure.WithoutEndTag)]
public class MetroTabTagHelper : MetroTagHelper, ITabItem
{
    public string Title { get; set; }
    public string Target { get; set; }
    public bool Active { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        if (Active) output.Attributes.AddClass("active");
        output.Content.AppendHtml($"<a href=\"#{Target}\">{Title}</a>");
        return Task.CompletedTask;
    }
}