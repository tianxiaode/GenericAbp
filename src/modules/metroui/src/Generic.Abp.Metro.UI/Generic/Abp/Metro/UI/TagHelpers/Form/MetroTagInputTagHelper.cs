using System.Data;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroTagInputTagHelper : MetroInputTagHelper
{
    public int? MaxTags { get; set; }
    public bool RandomColor { get; set; } = false;
    public string TagSeparator { get; set; }
    public string ClsTag { get; set; }
    public string ClsTagTitle { get; set; }
    public string ClsTagRemover { get; set; }

    public MetroTagInputTagHelper(HtmlEncoder htmlEncoder, IMetroTagHelperLocalizerService localizer,
        IHtmlGenerator generator) : base(htmlEncoder, localizer, generator)
    {
        IsTagInput = true;
    }

    protected override async Task<TagHelperOutput> GetInputTagHelperOutputAsync(TagHelperContext context,
        TagHelperOutput output)
    {
        var outputTagHelper = await base.GetInputTagHelperOutputAsync(context, output);
        await AddDataAttributeAsync(outputTagHelper);
        return outputTagHelper;
    }

    protected virtual async Task AddDataAttributeAsync(TagHelperOutput output)
    {
        var attribute = AspFor.ModelExplorer.GetAttribute<TagInput>();
        if (MaxTags.HasValue)
        {
            await AddDataAttributeAsync(output, nameof(MaxTags), MaxTags.Value);
        }
        else if (attribute.MaxTags.HasValue)
        {
            await AddDataAttributeAsync(output, nameof(MaxTags), attribute.MaxTags.Value);
        }

        if (RandomColor || attribute?.RandomColor == true)
        {
            await AddDataAttributeAsync(output, nameof(RandomColor), true);
        }

        var tagSeparator = string.IsNullOrWhiteSpace(TagSeparator) ? attribute?.TagSeparator : TagSeparator;
        if (!string.IsNullOrWhiteSpace(tagSeparator))
        {
            await AddDataAttributeAsync(output, nameof(TagSeparator), tagSeparator);
        }

        var clsTag = string.IsNullOrWhiteSpace(ClsTag) ? attribute?.ClsTag : ClsTag;
        if (!string.IsNullOrWhiteSpace(clsTag))
        {
            await AddDataAttributeAsync(output, nameof(ClsTag), clsTag);
        }

        var clsTagTitle = string.IsNullOrWhiteSpace(ClsTagTitle) ? attribute?.ClsTagTitle : ClsTagTitle;
        if (!string.IsNullOrWhiteSpace(clsTagTitle))
        {
            await AddDataAttributeAsync(output, nameof(ClsTagTitle), clsTagTitle);
        }

        var clsTagRemover = string.IsNullOrWhiteSpace(ClsTagRemover) ? attribute?.ClsTagRemover : ClsTagRemover;
        if (!string.IsNullOrWhiteSpace(clsTagRemover))
        {
            await AddDataAttributeAsync(output, nameof(ClsTagRemover), clsTagRemover);
        }
    }
}