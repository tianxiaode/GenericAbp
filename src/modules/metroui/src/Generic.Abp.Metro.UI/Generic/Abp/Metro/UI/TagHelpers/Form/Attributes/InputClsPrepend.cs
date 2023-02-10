using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputClsPrepend : Attribute
{
    public string ClsPrepend { get; set; }

    public InputClsPrepend(string clsPrepend)
    {
        ClsPrepend = clsPrepend;
    }
}