using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

public class MetroModalTagHelper : MetroTagHelper
{
    public const string Role = "modal";

    public MetroAccentColor AccentColor { get; set; } = MetroAccentColor.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddDataAttributeAsync(output, nameof(Role), Role);
        if (AccentColor != MetroAccentColor.Default)
        {
            output.Attributes.AddClass(AccentColor.ToString().ToLowerInvariant());
        }
    }
}