using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroInputTagHelper : MetroTagHelper<MetroInputTagHelper, MetroInputTagHelperService>
{
    public ModelExpression AspFor { get; set; }

    public string Label { get; set; }

    [HtmlAttributeName("info")]
    public string InfoText { get; set; }

    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = false;

    [HtmlAttributeName("readonly")]
    public bool IsReadonly { get; set; } = false;

    [HtmlAttributeName("type")]
    public string InputTypeName { get; set; }

    public MetroFormControlSize Size { get; set; } = MetroFormControlSize.Default;

    [HtmlAttributeName("required-symbol")]
    public bool DisplayRequiredSymbol { get; set; } = true;

    [HtmlAttributeName("asp-format")]
    public string Format { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public bool SuppressLabel { get; set; } = false;
    public CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }

    public MetroInputTagHelper(MetroInputTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
