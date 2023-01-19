using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelperService : MetroTagHelperService<MetroFormContentTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.Clear();
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetContent(MetroFormContentPlaceHolder);
    }
}
