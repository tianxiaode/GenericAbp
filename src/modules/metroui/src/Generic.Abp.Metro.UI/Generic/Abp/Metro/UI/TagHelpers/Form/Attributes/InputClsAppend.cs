using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputClsAppend : Attribute
{
    public string ClsAppend { get; set; }

    public InputClsAppend(string clsAppend)
    {
        ClsAppend = clsAppend;
    }
}