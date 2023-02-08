namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class FormContent
{
    public int Cols { get; set; } 
    public bool Horizontal { get; set; } 

    public LabelWidth LabelWidth { get; set; }
    public bool RequiredSymbols { get; set; }
    public FormContent(int cols, bool horizontal, LabelWidth labelWidth, bool requiredSymbols = true)
    {
        Cols = cols;
        Horizontal = horizontal;
        LabelWidth = labelWidth;
        RequiredSymbols = requiredSymbols;
    }
}