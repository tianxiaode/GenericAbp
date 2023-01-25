using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroCheckboxGroupTagHelper : MetroInputTagHelperBase<MetroCheckboxGroupTagHelper, MetroCheckboxGroupTagHelperService>, ISelectItemsTagHelper,IMetroInputTagHelperBase
{
    public MetroCheckboxGroupTagHelper(MetroCheckboxGroupTagHelperService service) : base(service)
    {
    }

    public bool? Disabled { get; set; }

    public IEnumerable<SelectListItem> AspItems { get; set; }

    public int? Cols { get; set; }


}