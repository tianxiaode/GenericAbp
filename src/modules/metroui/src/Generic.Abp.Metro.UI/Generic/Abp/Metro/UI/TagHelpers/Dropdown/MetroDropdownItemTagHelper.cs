namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownItemTagHelper : MetroTagHelper<MetroDropdownItemTagHelper, MetroDropdownItemTagHelperService>
{
    public bool? Active { get; set; }

    public bool? Disabled { get; set; }

    public MetroDropdownItemTagHelper(MetroDropdownItemTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
