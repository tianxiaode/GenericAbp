namespace Generic.Abp.Metro.UI.TagHelpers.List;

public class MetroListItem : IHasDisplayOrder, IMetroListItem
{
    public string Title { get; set; }
    public string Image { get; set; }
    public string Label { get; set; }
    public string SecondLabel { get; set; }
    public string SecondAction { get; set; }
    public string Marker { get; set; }
    public string StepContent { get; set; }
    public int DisplayOrder { get; set; }
}