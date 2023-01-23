using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement("metro-radio")]
public class MetroRadioInputTagHelper : MetroInputTagHelperBase<MetroRadioInputTagHelper,MetroRadioInputTagHelperService>, ISelectItemsTagHelper,IMetroInputTagHelperBase
{
    public bool? Inline { get; set; }

    public bool? Disabled { get; set; }

    public IEnumerable<SelectListItem> AspItems { get; set; }

    public MetroRadioInputTagHelper(MetroRadioInputTagHelperService service) : base(service)
    {
    }
}
