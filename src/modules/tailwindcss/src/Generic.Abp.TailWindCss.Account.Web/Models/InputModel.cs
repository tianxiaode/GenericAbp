using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Generic.Abp.TailWindCss.Account.Web.Models;

public class InputModel
{
    public IHtmlContent Input { get; set; }
    [CanBeNull] public IHtmlContent Label { get; set; }
    public LabelAlign LabelAlign { get; set; } = LabelAlign.Left;
    [CanBeNull] public string ControlCls { get; set; }
    [CanBeNull] public string IconCls { get; set; }
    [CanBeNull] public string InputCls { get; set; }
    public bool ShowClear { get; set; } = true;
    public string Placeholder { get; set; }
    public string Autocomplete { get; set; } = "off";

    public InputModel()
    {
    }

    public InputModel(
        IHtmlContent input,
        [CanBeNull] IHtmlContent label = null,
        LabelAlign labelAlign = LabelAlign.Left,
        [CanBeNull] string placeholder = null,
        string autocomplete = "off",
        [CanBeNull] string iconCls = null,
        [CanBeNull] string inputCls = null,
        [CanBeNull] string controlCls = null,
        bool showClear = true)
    {
        Input = input;
        Label = label;
        LabelAlign = labelAlign;
        Placeholder = placeholder;
        Autocomplete = autocomplete;
        IconCls = iconCls;
        InputCls = inputCls;
        ControlCls = controlCls;
        ShowClear = showClear;
    }
}

public enum LabelAlign
{
    Top,
    Left
}

public enum InputType
{
    Text,
    Email,
    Password,
    Number,
    Select
}