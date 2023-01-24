using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using NUglify.Css;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement("metro-radio")]
public class MetroRadioInputTagHelper : MetroInputTagHelperBase<MetroRadioInputTagHelper,MetroRadioInputTagHelperService>, ISelectItemsTagHelper,IMetroInputTagHelperBase
{

    public bool? Disabled { get; set; }

    public IEnumerable<SelectListItem> AspItems { get; set; }

    public int? Cols { get; set; }

    public MetroRadioInputTagHelper(MetroRadioInputTagHelperService service) : base(service)
    {
    }
}
