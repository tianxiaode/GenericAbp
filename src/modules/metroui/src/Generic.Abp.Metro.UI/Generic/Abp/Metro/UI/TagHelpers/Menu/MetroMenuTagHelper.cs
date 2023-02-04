using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

public class MetroMenuTagHelper : MetroTagHelper
{
    protected const string Role = "Dropdown";
    protected string IsMega { get; set; }
    public MenuType Type { get; set; } = MenuType.Horizontal;
    public MetroColor? BackgroundColor { get; set; }
    public MetroColor? Color { get; set; }
    public bool NoHover { get; set; } = false;
    public bool Large { get; set; } = false;
    public bool Mega { get; set; } = false;
    public bool Dropdown { get; set; } = false;
    public bool Open { get; set; } = false;
    public bool Compact { get; set; } = false;
    public bool Horizontal { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddClassesAsync(context, output);
    }

    protected virtual async Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cls = Type switch
        {
            MenuType.Horizontal => "h-menu",
            MenuType.Vertical => "v-menu",
            MenuType.Tool => "t-menu",
            _ => "d-menu"
        };

        var attributes = output.Attributes;
        attributes.AddClass(cls);
        if (Mega) attributes.AddClass("mega");
        if (Type == MenuType.Context) attributes.AddClass("context");
        if (NoHover) attributes.AddClass("no-hover");
        if (Large) attributes.AddClass("large");
        if (Type == MenuType.Tool)
        {
            if (Open) attributes.AddClass("open");
            if (Compact) attributes.AddClass("compact");
            if (Horizontal) attributes.AddClass("horizontal");
        }

        await AddColorClassAsync(output, BackgroundColor, true);
        await AddColorClassAsync(output, Color);
        if (Type == MenuType.Dropdown || Dropdown)
        {
            await AddDataAttributeAsync(output, nameof(Role), Role);
        }
    }
}