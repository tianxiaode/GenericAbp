using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputSize : Attribute
{
    public Form.InputSize Size { get; set; }

    public InputSize(Form.InputSize size)
    {
        Size = size;
    }
}