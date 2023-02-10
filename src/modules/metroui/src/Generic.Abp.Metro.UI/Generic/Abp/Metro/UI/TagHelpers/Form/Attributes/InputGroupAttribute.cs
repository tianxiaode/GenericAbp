using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

public abstract class InputGroupAttribute : Attribute
{
    public int? Cols { get; set; }
    public bool Disabled { get; set; }

    protected InputGroupAttribute()
    {
    }

    protected InputGroupAttribute(int cols)
    {
        Cols = cols;
    }

    protected InputGroupAttribute(bool disabled)
    {
        Disabled = disabled;
    }

    protected InputGroupAttribute(int cols, bool disabled)
    {
        Cols = cols;
        Disabled = disabled;
    }
}