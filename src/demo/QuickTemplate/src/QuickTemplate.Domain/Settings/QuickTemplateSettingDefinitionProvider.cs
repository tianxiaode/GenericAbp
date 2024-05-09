using Volo.Abp.Settings;

namespace QuickTemplate.Settings;

public class QuickTemplateSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(QuickTemplateSettings.MySetting1));
    }
}
