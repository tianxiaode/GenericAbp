using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement("abp-form-content", TagStructure = TagStructure.WithoutEndTag)]
public class AbpFormContentTagHelper : AbpTagHelper<AbpFormContentTagHelper, AbpFormContentTagHelperService>, ITransientDependency
{
    public AbpFormContentTagHelper(AbpFormContentTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
