using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputPrepend : Attribute
{
    public string Prepend { get; set; }

    public InputPrepend(string prepend)
    {
        Prepend = prepend;
    }
}