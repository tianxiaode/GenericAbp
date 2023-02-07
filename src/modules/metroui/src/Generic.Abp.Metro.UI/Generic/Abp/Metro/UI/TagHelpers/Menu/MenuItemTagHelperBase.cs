using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

public abstract class MenuItemTagHelperBase : MetroTagHelper
{
    protected virtual Task SetMainTagAsync(TagHelperContext context, TagHelperOutput output, string cls = "",
        bool isActive = false)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        if (!string.IsNullOrWhiteSpace(cls)) output.Attributes.AddClass(cls);
        if (isActive) output.Attributes.AddClass("active");
        return Task.CompletedTask;
    }

    protected virtual Task<string> GetAnchorAsHtmlAsync(string href, string cls = "", string icon = "",
        string text = "", string caption = "", string hotKey = "", string title = "", string counter = "",
        bool isDropDown = false)
    {
        var builder = new TagBuilder("a");
        if (!string.IsNullOrWhiteSpace(icon)) builder.InnerHtml.AppendHtml(icon);
        if (isDropDown) builder.AddCssClass("dropdown-toggle");
        if (!string.IsNullOrWhiteSpace(href)) builder.Attributes.Add("href", href);
        if (!string.IsNullOrWhiteSpace(cls)) builder.AddCssClass(cls);
        if (!string.IsNullOrWhiteSpace(text)) builder.InnerHtml.AppendHtml(text);
        if (!string.IsNullOrWhiteSpace(caption))
            builder.InnerHtml.AppendHtml($"<span class=\"caption\">{caption}</span>");
        if (!string.IsNullOrWhiteSpace(title))
            builder.InnerHtml.AppendHtml($"<span class=\"title\">{title}</span>");
        if (!string.IsNullOrWhiteSpace(counter))
            builder.InnerHtml.AppendHtml($"<span class=\"counter\">{counter}</span>");
        if (!string.IsNullOrWhiteSpace(hotKey)) builder.Attributes.Add("data-hotkey", hotKey);
        return Task.FromResult(builder.ToHtmlString());
    }

    protected virtual Task<string> GetIconAsHtmlAsync(string cls, bool isNavView = false)
    {
        var builder = new TagBuilder("span");
        builder.AddCssClass("icon");
        if (isNavView)
        {
            builder.InnerHtml.AppendHtml($"<span class=\"{cls}\"></span>");
        }
        else
        {
            builder.AddCssClass(cls);
        }

        return Task.FromResult(builder.ToHtmlString());
    }
}