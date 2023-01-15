using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Pagination;

[HtmlTargetElement("abp-paginator")]
public class AbpPaginationTagHelper : AbpTagHelper<AbpPaginationTagHelper, AbpPaginationTagHelperService>
{
    public PagerModel Model { get; set; }

    public bool? ShowInfo { get; set; }

    public AbpPaginationTagHelper(AbpPaginationTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
