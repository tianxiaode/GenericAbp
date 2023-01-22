using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelper : MetroTagHelper<MetroFormContentTagHelper, MetroFormContentTagHelperService>
{
    [HtmlAttributeName("abp-model")]
    public ModelExpression Model { get; set; }
    public int Cols { get; set; } = 1;
    public bool Horizontal { get; set; } = false;
    public int LabelWidth { get; set; } = 100;
    public MetroFormContentTagHelper(MetroFormContentTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
