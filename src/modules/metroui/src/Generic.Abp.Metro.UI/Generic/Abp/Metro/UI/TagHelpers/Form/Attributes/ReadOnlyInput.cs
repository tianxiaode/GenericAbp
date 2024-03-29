﻿using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ReadOnlyInput : Attribute
{
    public bool PlainText { get; set; }

    public ReadOnlyInput()
    {
    }

    public ReadOnlyInput(bool plainText)
    {
        PlainText = plainText;
    }
}