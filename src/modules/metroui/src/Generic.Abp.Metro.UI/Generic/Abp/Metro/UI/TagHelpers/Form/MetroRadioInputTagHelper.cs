using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement("metro-radio")]
public class MetroRadioInputTagHelper : MetroTagHelper<MetroRadioInputTagHelper, MetroRadioInputTagHelperService>
{
    public ModelExpression AspFor { get; set; }

    public string Label { get; set; }

    public bool? Inline { get; set; }

    public bool? Disabled { get; set; }

    public IEnumerable<SelectListItem> AspItems { get; set; }

    public MetroRadioInputTagHelper(MetroRadioInputTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
