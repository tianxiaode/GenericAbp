using Generic.Abp.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Extensions.Settings;

public abstract class SettingManagerBase(ISettingManager settingManager) : DomainService
{
    protected ISettingManager SettingManager { get; } = settingManager;


    public virtual async Task<Dictionary<string, object?>> GetAsync()
    {
        var result = new Dictionary<string, object?>();

        foreach (var (key, definition) in GetSettings())
        {
            result.Add(key.Replace(".", ""), await GetSettingAsync(key, definition.DefaultValue));
        }

        return result;
    }


    public virtual async Task UpdateAsync(Dictionary<string, object> newSettings, Guid? tenantId = null)
    {
        var defaultSettings = GetSettings();
        foreach (var (key, value) in newSettings)
        {
            var saveKey = defaultSettings.Keys.FirstOrDefault(m => m.Replace(".", "") == key.Capitalize());
            if (saveKey.IsNullOrEmpty())
            {
                continue;
            }

            await SetSettingAsync(saveKey, value);
        }
    }

    public virtual async Task<T> GetSettingAsync<T>(string name)
    {
        var defaultSettings = GetSettings();
        if (!defaultSettings.TryGetValue(name, out var definition))
        {
            return "".Parse<T>();
        }

        var stringValue = await GetSettingAsync(name, definition.DefaultValue);
        return stringValue.Parse<T>();
    }


    public virtual async Task<string> GetSettingAsync(string name, object? defaultValue)
    {
        var value = await SettingManager.GetOrNullForCurrentTenantAsync(GetGroupName() + "." + name);
        return value?.ToString() ?? defaultValue?.ToString() ?? string.Empty;
    }


    public virtual Task SetSettingAsync(string name, object value)
    {
        return SettingManager.SetForCurrentTenantAsync(GetGroupName() + "." + name, value?.ToString());
    }

    /// <summary>
    /// 获取设置定义，派生类可重写以提供具体设置。
    /// </summary>
    /// <returns>静态的设置定义列表。</returns>
    protected abstract Dictionary<string, ISettingDefinitionExtensions> GetSettings();

    protected abstract string GetGroupName();
}