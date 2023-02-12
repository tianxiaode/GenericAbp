using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CheckboxGroup : InputGroupAttribute
{
    public CheckboxGroup(int cols) : base(cols)
    {
    }
}