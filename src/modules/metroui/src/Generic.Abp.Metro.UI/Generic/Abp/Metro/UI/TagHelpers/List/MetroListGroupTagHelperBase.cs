using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.List;

public abstract class MetroListGroupTagHelperBase : MetroTagHelper
{
    public ListType Type { get; set; } = ListType.Group;
    public bool ImageOnRight { get; set; } = false;

    protected virtual Task<TagBuilder> AddItemAsync(string title, string marker, string stepContent, string image,
        string label, string secondLabel, string secondAction)
    {
        var tagBuilder = new TagBuilder("li");
        switch (Type)
        {
            case ListType.Marker:
                tagBuilder.InnerHtml.AppendHtml(title);
                tagBuilder.Attributes.Add("data-marker", marker);
                break;
            case ListType.Step:
                tagBuilder.InnerHtml.AppendHtml($"<h4>{title}</h4>");
                tagBuilder.InnerHtml.AppendHtml($"<p>{stepContent}</p>");
                break;
            case ListType.Items:
                tagBuilder.InnerHtml.AppendHtml($"<img class=\"avatar\" src=\"{image}\">");
                tagBuilder.InnerHtml.AppendHtml($"<span class=\"label\">{label}</span>");
                tagBuilder.InnerHtml.AppendHtml($"<span class=\"second-label\">{secondLabel}</span>");
                if (!string.IsNullOrWhiteSpace(secondAction))
                    tagBuilder.InnerHtml.AppendHtml($"<span class=\"second-action {secondAction}\"></span>");
                break;
            case ListType.Feed:
                var rightCls = ImageOnRight ? "on-right" : "";
                tagBuilder.InnerHtml.AppendHtml($"<img class=\"avatar {rightCls} \" src=\"{image}\">");
                tagBuilder.InnerHtml.AppendHtml($"<span class=\"label\">{label}</span>");
                tagBuilder.InnerHtml.AppendHtml($"<span class=\"second-label\">{secondLabel}</span>");
                break;
            case ListType.Group:
            default:
                tagBuilder.InnerHtml.AppendHtml(title);
                break;
        }

        return Task.FromResult(tagBuilder);
    }

    protected virtual Task AddFeedTitleAsync(StringBuilder builder, string title, int order)
    {
        if (Type != ListType.Feed || string.IsNullOrWhiteSpace(title)) return Task.CompletedTask;
        var style = order == 0 ? "" : $"style=\"order:{order - 1}\"";
        builder.AppendLine($"<li class=\"title flex-fill\" {style}>{title}</li>");

        return Task.CompletedTask;
    }
}