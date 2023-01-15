using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

[HtmlTargetElement("abp-modal-footer")]
public class AbpModalFooterTagHelper : AbpTagHelper<AbpModalFooterTagHelper, AbpModalFooterTagHelperService>
{
    public AbpModalButtons Buttons { get; set; }
    public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.Default;

    public AbpModalFooterTagHelper(AbpModalFooterTagHelperService tagHelperService)
        : base(tagHelperService)
    {
    }
}
