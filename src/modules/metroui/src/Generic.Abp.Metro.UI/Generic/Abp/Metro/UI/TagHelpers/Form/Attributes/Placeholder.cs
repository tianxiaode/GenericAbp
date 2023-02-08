using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class Placeholder : Attribute
{
    public string Value { get; set; }

    public Placeholder(string value)
    {
        Value = value;
    }
}