using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public abstract class MetroButtonTagHelperServiceBase<TTagHelper> : MetroTagHelperService<TTagHelper>
    where TTagHelper : TagHelper, IButtonTagHelperBase
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        NormalizeTagMode(context, output);
        AddClasses(context, output);
        AddIcon(context, output);
        AddText(context, output);
        AddDisabled(context, output);
    }

    protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
    }

    protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("button");

        if (TagHelper.ButtonType != MetroButtonType.Default)
        {
            output.Attributes.AddClass(TagHelper.ButtonType.ToString().ToLowerInvariant().Replace("_", "-"));
        }

        if (TagHelper.Size != MetroButtonSize.Default)
        {
            output.Attributes.AddClass(TagHelper.Size.ToString().ToLowerInvariant().Replace("_", "-"));
        }

    }

    protected virtual void AddIcon(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Icon.IsNullOrWhiteSpace())
        {
            return;
        }

        TagBuilder icon;
        if (TagHelper.IconType == FontIconType.Image)
        {
            icon = new TagBuilder("img");
            icon.Attributes.Add("src", TagHelper.Icon);
        }
        else
        {
            icon = new TagBuilder("span");
            icon.AddCssClass(GetIconClass(context, output));
            output.Content.AppendHtml(icon);
            output.Content.Append(" ");

        }

    }

    protected virtual string GetIconClass(TagHelperContext context, TagHelperOutput output)
    {
        return TagHelper.IconType switch
        {
            FontIconType.Metro => "mif-" + TagHelper.Icon,
            FontIconType.FontAwesome => "fa fa-" + TagHelper.Icon,
            _ => TagHelper.Icon
        };
    }

    protected virtual void AddText(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Text.IsNullOrWhiteSpace())
        {
            return;
        }

        output.Content.AppendHtml(TagHelper.Text);
    }

    protected virtual void AddDisabled(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Disabled ?? false)
        {
            output.Attributes.AddClass("disabled");
        }
    }
}
