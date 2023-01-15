using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class AbpRadioButton : Attribute
{
    public bool Inline { get; set; } = false;

    public bool Disabled { get; set; } = false;
}
