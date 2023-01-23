using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[OutputElementHint("select")]
public class MetroSelectTagHelper : MetroInputTagHelperBase<MetroSelectTagHelper, MetroSelectTagHelperService>, ISelectItemsTagHelper,IMetroInputTagHelperBase
{

    public IEnumerable<SelectListItem> AspItems { get; set; }


    [HtmlAttributeName("info")]
    public string InfoText { get; set; }

    public string AutocompleteApiUrl { get; set; }

    public string AutocompleteItemsPropertyName { get; set; }

    public string AutocompleteDisplayPropertyName { get; set; }

    public string AutocompleteValuePropertyName { get; set; }

    public string AutocompleteFilterParamName { get; set; }

    public string AutocompleteSelectedItemName { get; set; }

    public string AutocompleteSelectedItemValue { get; set; }

    public string AllowClear { get; set; }

    public string Placeholder { get; set; }

    public MetroSelectTagHelper(MetroSelectTagHelperService service) : base(service)
    {
    }
}
