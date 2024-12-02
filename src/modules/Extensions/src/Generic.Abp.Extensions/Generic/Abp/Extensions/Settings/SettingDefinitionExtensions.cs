using System;
using Generic.Abp.Extensions.Extensions;

namespace Generic.Abp.Extensions.Settings;

public interface ISettingDefinitionExtensions
{
    object? DefaultValue { get; set; }
    string? DisplayName { get; set; }
    bool IsVisibleToClients { get; set; }
    bool IsInherited { get; set; }
    bool IsEncrypted { get; set; }
}

[Serializable]
public class SettingDefinitionExtensions<T>(
    T defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false
) : ISettingDefinitionExtensions
{
    public object? DefaultValue { get; set; } = defaultValue;
    public string? DisplayName { get; set; } = displayName;
    public bool IsVisibleToClients { get; set; } = isVisibleToClients;
    public bool IsInherited { get; set; } = isInherited;
    public bool IsEncrypted { get; set; } = isEncrypted;

    public T ParseValue(string? stringValue)
    {
        return (stringValue ?? DefaultValue?.ToString()).Parse<T>();
    }
}

[Serializable]
public class SettingDefinitionExtensions(
    string defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<string>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class IntSettingDefinitionExtensions(
    int defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<int>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class LongSettingDefinitionExtensions(
    long defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<long>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class DoubleSettingDefinitionExtensions(
    double defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<double>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class DecimalSettingDefinitionExtensions(
    decimal defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<decimal>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class DateTimeSettingDefinitionExtensions(
    DateTime defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<DateTime>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);

[Serializable]
public class BooleanSettingDefinitionExtensions(
    bool defaultValue,
    string? displayName = null,
    bool isVisibleToClients = true,
    bool isInherited = true,
    bool isEncrypted = false)
    : SettingDefinitionExtensions<bool>(defaultValue, displayName, isVisibleToClients, isInherited, isEncrypted);