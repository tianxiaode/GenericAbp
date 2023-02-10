namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class FormContent
{
    public int Cols { get; set; }
    public bool Horizontal { get; set; }
    public bool RequiredSymbols { get; set; }
    public int LabelWidth { get; set; }
    public InputSize Size { get; set; }

    public FormContent()
    {
        Cols = 0;
        Horizontal = false;
        RequiredSymbols = true;
        LabelWidth = 100;
        Size = InputSize.Default;
    }

    public FormContent(int cols, bool horizontal, bool requiredSymbols = true, int labelWidth = 100,
        InputSize size = InputSize.Default)
    {
        Cols = cols;
        Horizontal = horizontal;
        RequiredSymbols = requiredSymbols;
        LabelWidth = labelWidth;
        Size = size;
    }
}