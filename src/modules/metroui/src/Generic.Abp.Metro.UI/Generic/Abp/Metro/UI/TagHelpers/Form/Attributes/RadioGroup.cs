using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RadioGroup : InputGroupAttribute
{
    public RadioGroup()
    {
    }

    public RadioGroup(int cols) : base(cols)
    {
    }

    public RadioGroup(bool disabled) : base(disabled)
    {
    }

    public RadioGroup(int cols, bool disabled) : base(cols, disabled)
    {
    }
}