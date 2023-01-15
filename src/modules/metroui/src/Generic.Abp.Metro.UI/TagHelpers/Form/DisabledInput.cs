using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class DisabledInput : Attribute
{
    public DisabledInput()
    {
    }
}
