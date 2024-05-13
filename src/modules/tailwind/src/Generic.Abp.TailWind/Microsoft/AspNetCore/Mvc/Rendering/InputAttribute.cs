using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Generic.Abp.Tailwind.Microsoft.AspNetCore.Mvc.Rendering;

public class InputAttribute(
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
    public int InputClos { get; } = 12 - labelCols;
    public string? Placeholder { get; set; } = placeholder;

    public string GetStartCls()
    {
        return InputClos == 12 ? "" : $"col-start-{13 - InputClos}";
    }
}