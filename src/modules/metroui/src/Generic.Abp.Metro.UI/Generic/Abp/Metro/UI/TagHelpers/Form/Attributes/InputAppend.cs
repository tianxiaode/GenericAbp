using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputAppend : Attribute
{
    public string Append { get; set; }

    public InputAppend(string append)
    {
        Append = append;
    }
}