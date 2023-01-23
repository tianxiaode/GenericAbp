using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public interface ISelectItemsTagHelper: ITagHelper
{
    ModelExpression AspFor { get; set; }
    IEnumerable<SelectListItem> AspItems { get; set; }
}