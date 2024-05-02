using Volo.Abp.Settings;

namespace Generic.Abp.Host.Settings;

public class HostSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(HostSettings.MySetting1));
    }
}
