using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

[HtmlTargetElement("metro-menu-item", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroMenuItemTagHelper : MenuItemTagHelperBase
{
    public MenuItemType Type { get; set; } = MenuItemType.Default;
    public string Text { get; set; }
    public string Href { get; set; }
    public string Icon { get; set; }
    public string MegaContainerCls { get; set; }
    public string HotKey { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await SetMainTagAsync(context, output);
        if (Type == MenuItemType.Mega)
        {
            await AddFlagValueAsync(context, nameof(MetroMenuTagHelper), TagHelperConsts.MenuIsMegaName);
        }

        var child = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(await GetContentAsync(context, output, child));
    }

    protected virtual async Task<string> GetContentAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent child)
    {
        var childContent = child.GetContent();

        var stringBuilder = new StringBuilder();
        switch (Type)
        {
            case MenuItemType.Divider:
                output.Attributes.AddClass("divider");
                break;
            case MenuItemType.Title:
                output.Attributes.AddClass("menu-title");
                var title = string.IsNullOrEmpty(Text) ? childContent : Text;
                stringBuilder.AppendLine(title);
                break;
            case MenuItemType.DropDown:
                stringBuilder.AppendLine(await GetAnchorAsHtmlAsync(child));
                stringBuilder.AppendLine(childContent);
                break;
            case MenuItemType.Mega:
                stringBuilder.AppendLine(await GetAnchorAsHtmlAsync(child));
                var cls = MegaContainerCls ?? string.Empty;
                stringBuilder.AppendLine($"<div class=\"mega-container {cls} \" data-role=\"dropdown\">");
                stringBuilder.AppendLine(childContent);
                stringBuilder.AppendLine("</div>");
                break;
            case MenuItemType.Custom:
                stringBuilder.AppendLine(childContent);
                break;
            case MenuItemType.Default:
            default:
                stringBuilder.AppendLine(await GetAnchorAsHtmlAsync(child));
                break;
        }

        return stringBuilder.ToString();
    }

    protected virtual async Task<string> GetAnchorAsHtmlAsync(TagHelperContent childContent)
    {
        var text = Text;
        if (string.IsNullOrWhiteSpace(text) && Type is MenuItemType.Default)
            text = childContent.GetContent();
        var iconHtml = "";
        if (!string.IsNullOrWhiteSpace(Icon))
        {
            iconHtml = await GetIconAsHtmlAsync(Icon);
        }

        return await GetAnchorAsHtmlAsync(Href, icon: iconHtml, text: text, hotKey: HotKey,
            isDropDown: Type is MenuItemType.DropDown or MenuItemType.Mega);
    }
}