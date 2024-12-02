using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.Extensions.Settings;

public abstract class SettingDefinitionExtensionsProvider<TResource> : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var groupNames = GetGroupName();
        foreach (var (key, definition) in GetSettings())
        {
            var displayName = definition.DisplayName ?? key;
            context.Add(
                new SettingDefinition(
                    groupNames + "." + key,
                    definition.DefaultValue?.ToString(),
                    L($"Settings:{displayName}"),
                    L($"Description:{displayName}"),
                    definition.IsVisibleToClients,
                    definition.IsInherited,
                    definition.IsEncrypted
                )
            );
        }
    }

    protected static LocalizableString L(string name)
    {
        return LocalizableString.Create<TResource>(name);
    }

    protected abstract Dictionary<string, ISettingDefinitionExtensions> GetSettings();
    protected abstract string GetGroupName();
}