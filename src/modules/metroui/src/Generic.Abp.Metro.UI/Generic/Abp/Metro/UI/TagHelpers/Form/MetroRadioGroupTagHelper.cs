using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroRadioGroupTagHelper : MetroInputTagHelperBase<MetroRadioGroupTagHelper,MetroRadioGroupTagHelperService>, ISelectItemsTagHelper,IMetroInputTagHelperBase
{

    public bool? Disabled { get; set; }

    public IEnumerable<SelectListItem> AspItems { get; set; }

    public int? Cols { get; set; }

    public MetroRadioGroupTagHelper(MetroRadioGroupTagHelperService service) : base(service)
    {
    }
}
