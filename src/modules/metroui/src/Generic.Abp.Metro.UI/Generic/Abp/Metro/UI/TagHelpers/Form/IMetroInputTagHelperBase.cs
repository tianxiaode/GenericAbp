using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public interface IMetroInputTagHelperBase
{
    ModelExpression AspFor { get; set; }
    bool IsDisabled { get; set; }
    bool IsReadonly { get; set; }
    string Label { get; set; }
    string InputTypeName { get; set; }
    bool DisplayRequiredSymbol { get; set; }
    string Format { get; set; }
    string Name { get; set; }
    string Value { get; set; }
    bool SuppressLabel { get; set; }
    CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }
    MetroFormControlSize Size { get; set; }
    ViewContext ViewContext { get; set; }

}