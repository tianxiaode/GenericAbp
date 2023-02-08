using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

[Flags]
public enum ModalDialogButtons
{
    None = 0,
    Save = 1,
    Cancel = 2,
    Close = 4
}