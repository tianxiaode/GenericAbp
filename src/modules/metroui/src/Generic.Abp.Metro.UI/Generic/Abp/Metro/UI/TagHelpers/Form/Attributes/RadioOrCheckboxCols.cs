using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RadioOrCheckboxCols : Attribute
{
    public int Cols { get; set; }

    public RadioOrCheckboxCols(int cols)
    {
        Cols = cols;
    }
}