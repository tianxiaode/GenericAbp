using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public abstract class MetroButtonTagHelperServiceBase<TTagHelper> : MetroTagHelperService<TTagHelper>
    where TTagHelper : TagHelper, IButtonTagHelperBase
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await NormalizeTagModeAsync(context, output);
        await AddClassesAsync(context, output);
        await AddIconAsync(context, output);
        await AddTextAsync(context, output);
        await AddDisabledAsync(context, output);
    }

    protected virtual Task NormalizeTagModeAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        return Task.CompletedTask;
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
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
        return Task.CompletedTask;
    }

    protected virtual async Task AddIconAsync(TagHelperContext context, TagHelperOutput output)
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
            icon.AddCssClass(await GetIconClassAsync(context, output));
            output.Content.AppendHtml(icon);
            output.Content.Append(" ");

        }
    }

    protected virtual Task<string> GetIconClassAsync(TagHelperContext context, TagHelperOutput output)
    {
        var iconCls = TagHelper.IconType switch
        {
            FontIconType.Metro => "mif-" + TagHelper.Icon,
            FontIconType.FontAwesome => "fa fa-" + TagHelper.Icon,
            _ => TagHelper.Icon
        };
        return Task.FromResult(iconCls);
    }

    protected virtual Task AddTextAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Text.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        output.Content.AppendHtml(TagHelper.Text);
        return Task.CompletedTask;
    }

    protected virtual Task AddDisabledAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Disabled ?? false)
        {
            output.Attributes.AddClass("disabled");
        }
        return Task.CompletedTask;
    }
}
