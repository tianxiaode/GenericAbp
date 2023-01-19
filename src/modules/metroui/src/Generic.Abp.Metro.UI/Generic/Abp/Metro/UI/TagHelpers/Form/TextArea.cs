using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class TextArea : Attribute
{
    public int Rows { get; set; } = -1;

    public int Cols { get; set; } = -1;

    public TextArea()
    {
    }
}
