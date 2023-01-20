namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class FormContent
{
    public int Cols { get; set; }
    public bool Horizontal { get; set; } = false;

    public int LabelWidth { get; set; }

    public FormContent(int cols, bool horizontal, int labelWidth)
    {
        Cols = cols;
        Horizontal = horizontal;
        LabelWidth = labelWidth;
    }
}