using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public abstract class ButtonTagHelperBase : MetroTagHelper, IButtonTagHelperBase
{
    public MetroAccentColor AccentColor { get; set; } = MetroAccentColor.Default;
    public MetroButtonSize Size { get; set; } = MetroButtonSize.Default;
    public ButtonShape Shape { get; set; } = ButtonShape.Default;
    public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Default;
    public string Text { get; set; }
    public string Icon { get; set; }
    public string IconCls { get; set; }
    public bool? Disabled { get; set; }
    public FontIconType IconType { get; set; } = FontIconType.Metro;
    public bool? Outline { get; set; }
    public bool? Rounded { get; set; }
    public bool? Shadowed { get; set; }
    public string Caption { get; set; }
    public bool? IconRight { get; set; }
    public HintPosition HintPosition { get; set; } = HintPosition.Top;
    public string HintText { get; set; }
    public string ClsHint { get; set; }


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        await ProcessHintAsync(context, output);
        await AddClassesAsync(context, output);
        await AddIconAsync(context, output);
        await AddTextAsync(context, output);
        await AddDisabledAsync(context, output);
        await AddCaptionAsync(context, output);
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        var attributes = output.Attributes;
        var styleClass = ButtonStyle switch
        {
            ButtonStyle.Shortcut => "shortcut",
            ButtonStyle.Image => "image-button",
            ButtonStyle.Command => "command-button",
            ButtonStyle.Flat => "button flat-button",
            ButtonStyle.Tool => "tool-button",
            _ => "button"
        };


        attributes.AddClass(styleClass);

        if (IconRight == true && ButtonStyle is ButtonStyle.Image or ButtonStyle.Command)
        {
            attributes.AddClass("icon-right");
        }

        if (AccentColor != MetroAccentColor.Default)
        {
            attributes.AddClass(Enum.GetName(AccentColor)?.ToLowerInvariant());
        }


        if (Size != MetroButtonSize.Default)
        {
            attributes.AddClass(Enum.GetName(Size)?.ToLowerInvariant());
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
            attributes.AddClass(Enum.GetName(Shape)?.ToLowerInvariant());
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
        if (!IconCls.IsNullOrWhiteSpace()) icon.AddCssClass(IconCls);
        if (ButtonStyle is ButtonStyle.Command or ButtonStyle.Image or ButtonStyle.Shortcut)
        {
            icon.AddCssClass("icon");
        }

        if (!string.IsNullOrWhiteSpace(Text))
        {
            var cls = IconRight == true ? "" : "pr-1";
            icon.AddCssClass(cls);
        }

        output.Content.AppendHtml(icon);

        return Task.CompletedTask;
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

    protected virtual Task AddCaptionAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (ButtonStyle is ButtonStyle.Flat or ButtonStyle.Default or ButtonStyle.Tool)
            return Task.CompletedTask;
        var caption = new TagBuilder("span");
        caption.AddCssClass("caption");
        caption.InnerHtml.AppendHtml(Caption);
        output.Content.AppendHtml(caption);
        return Task.CompletedTask;
    }

    protected virtual async Task ProcessHintAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (ButtonStyle != ButtonStyle.Hint) return;
        const string role = "hint";
        await AddDataAttributeAsync(output, nameof(role), role);
        await AddDataAttributeAsync(output, nameof(HintText), HintText);
        await AddDataAttributeAsync(output, nameof(ClsHint), ClsHint);
        await AddDataAttributeAsync(output, nameof(HintPosition), HintPosition);
    }
}