using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogFooterTagHelper : MetroTagHelper<MetroDialogFooterTagHelper, MetroDialogFooterTagHelperService>
{
    public MetroDialogButtons Buttons { get; set; }
    public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.End;

    public MetroDialogFooterTagHelper(MetroDialogFooterTagHelperService tagHelperService)
        : base(tagHelperService)
    {
    }
}
