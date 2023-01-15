
using Generic.Abp.Metro.UI.TagHelpers.Grid;

namespace Generic.Abp.Metro.UI.TagHelpers.Tab;

public class AbpTabsTagHelper : AbpTagHelper<AbpTabsTagHelper, AbpTabsTagHelperService>
{
    public string Name { get; set; }

    public TabStyle TabStyle { get; set; } = TabStyle.Tab;

    public ColumnSize VerticalHeaderSize { get; set; } = ColumnSize._3;

    public AbpTabsTagHelper(AbpTabsTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
