using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

public abstract class InputGroupAttribute : Attribute
{
    public int? Cols { get; set; }
    public bool Disabled { get; set; } = false;
}