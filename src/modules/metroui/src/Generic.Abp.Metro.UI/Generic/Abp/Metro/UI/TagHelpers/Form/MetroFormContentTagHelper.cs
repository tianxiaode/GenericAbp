using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelper : MetroTagHelper<MetroFormContentTagHelper, MetroFormContentTagHelperService>
{
    [HtmlAttributeName("metro-model")]
    public ModelExpression Model { get; set; }
    public int? Cols { get; set; }
    public bool? Horizontal { get; set; }
    public LabelWidth? LabelWidth { get; set; }
    public bool? RequiredSymbols { get; set; }
    public MetroFormContentTagHelper(MetroFormContentTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
