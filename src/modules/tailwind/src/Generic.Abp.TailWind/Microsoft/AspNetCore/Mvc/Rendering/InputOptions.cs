﻿namespace Generic.Abp.Tailwind.Microsoft.AspNetCore.Mvc.Rendering;

public class InputOptions(
    int labelCols = 3,
    InputType type = InputType.Text,
    object? formControlAttributes = null,
    object? labelAttributes = null,
    object? inputWrapAttributes = null,
    object? inputAttributes = null,
    object? errorAttributes = null,
    bool clearable = true,
    bool hasPasswordIndicator = true,
    string autocomplete = "off",
    string? placeholder = "auto",
    bool requiredSymbol = false,
    string? labelText = null,
    string? iconCls = null)
{
    public InputType Type { get; set; } = type;
    public int LabelCols { get; set; } = labelCols;
    public object? FormControlAttributes { get; set; } = formControlAttributes;
    public object? LabelAttributes { get; set; } = labelAttributes;
    public object? InputWrapAttributes { get; set; } = inputWrapAttributes;
    public object? InputAttributes { get; set; } = inputAttributes;
    public object? ErrorAttributes { get; set; } = errorAttributes;
    public bool Clearable { get; set; } = clearable;
    public bool HasPasswordIndicator { get; set; } = hasPasswordIndicator;
    public string Autocomplete { get; set; } = autocomplete;
    public string? IconCls { get; set; } = iconCls;
    public int InputClos { get; } = labelCols == 12 ? 12 : 12 - labelCols;
    public string? Placeholder { get; set; } = placeholder;
    public bool RequiredSymbol { get; set; } = requiredSymbol;
    public string? LabelText { get; set; } = labelText;

    public string GetStartCls()
    {
        return InputClos == 12 ? "" : $"col-start-{13 - InputClos}";
    }
}