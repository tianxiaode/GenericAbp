using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CheckboxGroup : InputGroupAttribute
{
    public CheckboxGroup()
    {
    }

    public CheckboxGroup(int cols) : base(cols)
    {
    }

    public CheckboxGroup(bool disabled) : base(disabled)
    {
    }

    public CheckboxGroup(int cols, bool disabled) : base(cols, disabled)
    {
    }
}