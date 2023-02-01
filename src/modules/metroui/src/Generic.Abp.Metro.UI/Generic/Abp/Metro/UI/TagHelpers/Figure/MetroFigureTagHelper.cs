using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Figure;

[HtmlTargetElement("metro-figure", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroFigureTagHelper : MetroTagHelper
{
    public string Image { get; set; }
    public string Caption { get; set; }
    public FigureCaptionAlignment CaptionAlignment { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "figure";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml($"<img src=\"{Image}\" />");
        var captionCls = CaptionAlignment switch
        {
            FigureCaptionAlignment.Center => "text-center",
            FigureCaptionAlignment.Right => "text-right",
            _ => ""
        };
        var captionHtml = $"<figcaption class=\"{captionCls}\">{Caption}</figcaption>";
        output.Content.AppendHtml(captionHtml);
        return Task.CompletedTask;
    }
}