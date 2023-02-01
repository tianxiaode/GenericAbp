using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavigationMenuTagHelperBase : MetroTagHelper
{
    protected virtual Task<TagBuilder> AddItemAsync(MetroNavigationMenuItemType type, string text, string value,
        string url, string icon,
        string currentValue)
    {
        var tagBuilder = new TagBuilder("li");
        var innerHtml = tagBuilder.InnerHtml;
        switch (type)
        {
            case MetroNavigationMenuItemType.Separator:
                tagBuilder.AddCssClass("item-separator");
                break;
            case MetroNavigationMenuItemType.Header:
                tagBuilder.AddCssClass("item-header");
                innerHtml.AppendHtml(text);
                break;
            case MetroNavigationMenuItemType.Default:
            default:
                if (!string.IsNullOrWhiteSpace(value) && value.Equals(currentValue, StringComparison.OrdinalIgnoreCase))
                {
                    tagBuilder.AddCssClass("active");
                }

                var strBuilder = new StringBuilder();
                strBuilder.AppendLine($"<a href=\"{url}\">");
                if (!string.IsNullOrWhiteSpace(icon))
                {
                    strBuilder.AppendLine($"<span class=\"icon\"><span class=\"{icon}\"></span></span>");
                }

                strBuilder.AppendLine($" <span class=\"caption\">{text}</span>");
                strBuilder.AppendLine("</a>");
                innerHtml.AppendHtml(strBuilder?.ToString());
                break;
        }


        return Task.FromResult(tagBuilder);
    }
}