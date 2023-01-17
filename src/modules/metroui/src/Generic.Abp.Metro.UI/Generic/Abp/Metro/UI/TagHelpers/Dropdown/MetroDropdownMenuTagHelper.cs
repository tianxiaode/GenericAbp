namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownMenuTagHelper : MetroTagHelper<MetroDropdownMenuTagHelper, MetroDropdownMenuTagHelperService>
{
    public DropdownDirection Direction { get; set; } = DropdownDirection.Down;

    public MetroDropdownMenuTagHelper(MetroDropdownMenuTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
