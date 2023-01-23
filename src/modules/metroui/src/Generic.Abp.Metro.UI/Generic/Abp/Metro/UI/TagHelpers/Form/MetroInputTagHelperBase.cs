using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

//public abstract class MetroTagHelper<TTagHelper, TService> : MetroTagHelper
//    where TTagHelper : MetroTagHelper<TTagHelper, TService>
//    where TService : class, IMetroTagHelperService<TTagHelper>

public abstract class MetroInputTagHelperBase<TTagHelper, TService> : MetroTagHelper<TTagHelper, TService>
    where TTagHelper : MetroTagHelper<TTagHelper, TService>
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    public ModelExpression AspFor { get; set; }
    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = false;

    [HtmlAttributeName("readonly")]
    public bool IsReadonly { get; set; } = false;
    public string Label { get; set; }

    [HtmlAttributeName("type")]
    public string InputTypeName { get; set; }

    [HtmlAttributeName("required-symbol")]
    public bool DisplayRequiredSymbol { get; set; } = true;

    [HtmlAttributeName("asp-format")]
    public string Format { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public bool SuppressLabel { get; set; } = false;
    public CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }

    public MetroFormControlSize Size { get; set; } = MetroFormControlSize.Default;

    protected MetroInputTagHelperBase(TService service) : base(service)
    {
    }
}