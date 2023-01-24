using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class MetroRadioButton : Attribute
{
    public bool Disabled { get; set; } = false;
}
