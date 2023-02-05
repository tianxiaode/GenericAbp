namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

public class MenuGroupItem : GroupItem
{
    public bool IsMega { get; set; }
    public int Depth { get; set; }

    public MenuGroupItem()
    {
    }

    public MenuGroupItem(int depth)
    {
        IsMega = true;
        Depth = depth;
    }
}