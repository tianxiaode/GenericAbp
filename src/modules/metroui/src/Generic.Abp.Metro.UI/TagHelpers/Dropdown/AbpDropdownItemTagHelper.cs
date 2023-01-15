namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class AbpDropdownItemTagHelper : AbpTagHelper<AbpDropdownItemTagHelper, AbpDropdownItemTagHelperService>
{
    public bool? Active { get; set; }

    public bool? Disabled { get; set; }

    public AbpDropdownItemTagHelper(AbpDropdownItemTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
