using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

[HtmlTargetElement("metro-menu-divider", TagStructure = TagStructure.WithoutEndTag)]
public class MetroMenuDividerTagHelper : MetroMenuTagHelper
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("divider");
        return Task.CompletedTask;
    }
}