namespace Generic.Abp.Metro.UI.TagHelpers.List;

public interface IMetroListItem
{
    string Title { get; }
    string Image { get; }
    string Label { get; }
    string SecondLabel { get; }
    string SecondAction { get; }
    string Marker { get; }
    string StepContent { get; }
}