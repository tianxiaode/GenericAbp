using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement(Attributes = "asp-validation-for")]
[HtmlTargetElement(Attributes = "asp-validation-summary")]
public class AbpValidationAttributeTagHelper : AbpTagHelper<AbpValidationAttributeTagHelper, AbpValidationAttributeTagHelperService>, ITransientDependency
{
    public AbpValidationAttributeTagHelper(AbpValidationAttributeTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
