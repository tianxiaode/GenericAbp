using System;
using System.Linq;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public abstract class ButtonTagHelperBase : TagHelper, IButtonTagHelperBase
{
    public MetroButtonType ButtonType { get; set; } = MetroButtonType.Default;
    public MetroButtonSize Size { get; set; } = MetroButtonSize.Default;
    public ButtonShape Shape { get; set; } = ButtonShape.Default;
    public string Text { get; set; }
    public string Icon { get; set; }
    public bool? Disabled { get; set; }
    public FontIconType IconType { get; set; } = FontIconType.Metro;
    public bool? Outline { get; set; }
    public bool? Rounded { get; set; }
    public bool? Shadowed { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddClassesAsync(context, output);
        await AddIconAsync(context, output);
        await AddTextAsync(context, output);
        await AddDisabledAsync(context, output);
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        var attributes = output.Attributes;
        attributes.AddClass("button");

        if (ButtonType != MetroButtonType.Default)
        {
            attributes.AddClass(Enum.GetName(ButtonType)?.ToLowerInvariant());
        }

        if (Size != MetroButtonSize.Default)
        {
            attributes.AddClass(nameof(Size));
        }

        if (Outline == true)
        {
            attributes.AddClass("outline");
        }

        if (Rounded == true)
        {
            attributes.AddClass("rounded");
        }

        if (Shadowed == true)
        {
            attributes.AddClass("shadowed");
        }

        if (Shape != ButtonShape.Default)
        {
            attributes.AddClass(nameof(Shape));
        }

        return Task.CompletedTask;
    }

    protected virtual Task AddIconAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Icon.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        if (IconType == FontIconType.Image)
        {
            var image = new TagBuilder("image");
            image.Attributes.Add("src", Icon);
            output.Content.AppendHtml(image);
            return Task.CompletedTask;
        }

        var icon = new TagBuilder("span");
        icon.AddCssClass(GetIconClass(context, output));
        AddIconElementClass(output, icon);
        output.Content.AppendHtml(icon);

        return Task.CompletedTask;
    }

    protected virtual void AddIconElementClass(TagHelperOutput output, TagBuilder tagBuilder)
    {
        var special = new[] { "command-button", "image-button", "shortcut" };
        if (output.Attributes.Any(m => special.Any(n => n == m.Value.ToString())))
        {
            tagBuilder.AddCssClass("icon");
        }
    }

    protected virtual string GetIconClass(TagHelperContext context, TagHelperOutput output)
    {
        return IconType switch
        {
            FontIconType.Metro => "mif-" + Icon,
            FontIconType.FontAwesome => "fa fa-" + Icon,
            _ => Icon
        };
    }

    protected virtual Task AddTextAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Text.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        output.Content.AppendHtml(Text);
        return Task.CompletedTask;
    }

    protected virtual Task AddDisabledAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Disabled ?? false)
        {
            output.Attributes.Add("disabled", "disabled");
        }

        return Task.CompletedTask;
    }
}