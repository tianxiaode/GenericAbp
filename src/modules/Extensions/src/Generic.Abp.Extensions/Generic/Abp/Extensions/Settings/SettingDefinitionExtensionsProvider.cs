using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.Extensions.Settings;

public abstract class SettingDefinitionExtensionsProvider<TResource> : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var groupNames = GetGroupName();
        foreach (var pair in GetSettings())
        {
            var key = pair.Key;
            var definition = pair.Value;

            // 继续使用 key 和 definition
            var displayName = definition.DisplayName ?? key;
            var settingKey = $"{groupNames}.{key}";

            context.Add(new SettingDefinition(
                settingKey,
                definition.DefaultValue?.ToString(),
                L($"Settings:{displayName}"),
                L($"Description:{displayName}"),
                definition.IsVisibleToClients,
                definition.IsInherited,
                definition.IsEncrypted
            ));
        }
    }

    protected static LocalizableString L(string name)
    {
        return LocalizableString.Create<TResource>(name);
    }

    protected abstract Dictionary<string, ISettingDefinitionExtensions> GetSettings();
    protected abstract string GetGroupName();
}