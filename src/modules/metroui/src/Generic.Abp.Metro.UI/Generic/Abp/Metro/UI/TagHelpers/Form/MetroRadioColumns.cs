using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class MetroRadioColumns: Attribute
{
    public int Columns { get; set; }

    public MetroRadioColumns(int columns)
    {
        Columns = columns;
    }
}