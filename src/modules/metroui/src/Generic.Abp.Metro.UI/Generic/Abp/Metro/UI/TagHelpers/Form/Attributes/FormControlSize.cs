using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FormControlSize : Attribute
{
    public MetroFormControlSize Size { get; set; }

    public FormControlSize(MetroFormControlSize size)
    {
        Size = size;
    }
}