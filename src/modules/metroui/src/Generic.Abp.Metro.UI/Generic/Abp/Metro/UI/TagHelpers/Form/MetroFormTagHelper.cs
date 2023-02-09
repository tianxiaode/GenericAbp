using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormTagHelper : FormTagHelper
{
    public MetroFormTagHelper(IHtmlGenerator generator) : base(generator)
    {
    }
}