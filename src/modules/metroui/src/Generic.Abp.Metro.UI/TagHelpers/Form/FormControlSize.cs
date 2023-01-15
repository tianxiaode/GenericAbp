using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class FormControlSize : Attribute
{
    public AbpFormControlSize Size { get; set; }

    public FormControlSize(AbpFormControlSize size)
    {
        Size = size;
    }
}
