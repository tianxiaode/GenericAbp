using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class InputInfoText : Attribute
{
    public string Text { get; set; }

    public InputInfoText(string text)
    {
        Text = text;
    }
}
