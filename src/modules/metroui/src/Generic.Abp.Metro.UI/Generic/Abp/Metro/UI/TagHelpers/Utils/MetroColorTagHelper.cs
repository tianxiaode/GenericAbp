using System;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

public abstract class MetroColorTagHelper : TagHelper
{
    protected virtual Task AddColorClassAsync(TagHelperOutput output, MetroColor value, bool isBackGround = false)
    {
        var cls = value.ToString();
        if (string.IsNullOrWhiteSpace(cls)) return Task.CompletedTask;
        cls = (isBackGround ? TagHelperConsts.BackgroundColorPrefix : TagHelperConsts.ForegroundColorPrefix) +
              cls[..1].ToLowerInvariant() + cls[1..];
        output.Attributes.AddClass(cls);
        return Task.CompletedTask;
    }
}