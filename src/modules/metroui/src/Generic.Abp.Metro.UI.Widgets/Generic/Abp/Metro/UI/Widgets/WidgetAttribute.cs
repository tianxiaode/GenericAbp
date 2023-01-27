using System;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Volo.Abp;

namespace Generic.Abp.Metro.UI.Widgets;

[AttributeUsage(AttributeTargets.Class)]
public class WidgetAttribute : Attribute
{
    public string[] StyleFiles { get; set; }

    public Type[] StyleTypes { get; set; }

    public string[] ScriptFiles { get; set; }

    public Type[] ScriptTypes { get; set; }

    public string DisplayName { get; set; }

    public Type DisplayNameResource { get; set; }

    public string[] RequiredPolicies { get; set; }

    public bool RequiresAuthentication { get; set; }

    public string RefreshUrl { get; set; } = string.Empty;

    public bool AutoInitialize { get; set; }

    public static bool IsWidget(Type type)
    {
        return type.IsSubclassOf(typeof(ViewComponent)) &&
               type.IsDefined(typeof(WidgetAttribute), true);
    }

    public static WidgetAttribute Get(Type viewComponentType)
    {
        return viewComponentType.GetCustomAttribute<WidgetAttribute>(true)
               ?? throw new AbpException(
                   $"Given color '{viewComponentType.AssemblyQualifiedName}' does not declare a {typeof(WidgetAttribute).AssemblyQualifiedName}");
    }
}