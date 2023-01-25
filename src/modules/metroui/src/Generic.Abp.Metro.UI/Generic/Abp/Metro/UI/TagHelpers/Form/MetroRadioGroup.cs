using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class MetroRadioGroup : Attribute
{
    public bool Disabled { get; set; }
    public int Cols { get; set; }

    public MetroRadioGroup(bool disabled = false, int cols = 1)
    {
        Disabled = disabled;
        Cols = cols;
    }
}
