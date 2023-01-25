using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class MetroRadioOrCheckboxCols: Attribute
{
    public int Cols { get; set; }

    public MetroRadioOrCheckboxCols(int cols)
    {
        Cols = cols;
    }
}