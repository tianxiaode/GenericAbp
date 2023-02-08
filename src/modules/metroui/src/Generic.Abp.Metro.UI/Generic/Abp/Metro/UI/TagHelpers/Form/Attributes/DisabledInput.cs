using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DisabledInput : Attribute
{
    public DisabledInput()
    {
    }
}