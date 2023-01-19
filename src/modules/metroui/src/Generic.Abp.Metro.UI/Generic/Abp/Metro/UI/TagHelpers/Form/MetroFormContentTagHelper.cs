using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement("abp-form-content", TagStructure = TagStructure.WithoutEndTag)]
public class MetroFormContentTagHelper : MetroTagHelper<MetroFormContentTagHelper, MetroFormContentTagHelperService>, ITransientDependency
{
    public int Cols { get; set; } = 1;
    public bool Horizontal { get; set; } = false;

    public MetroFormContentTagHelper(MetroFormContentTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
