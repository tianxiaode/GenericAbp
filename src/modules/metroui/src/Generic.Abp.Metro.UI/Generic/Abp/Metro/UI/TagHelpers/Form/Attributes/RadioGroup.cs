using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RadioGroup : InputGroupAttribute
{
    public RadioGroup(int cols) : base(cols)
    {
    }
}