using Generic.Abp.Metro.UI.TagHelpers.Form;
using Volo.Abp.Settings;

namespace Generic.Abp.Metro.UI.Settings
{
    public class MetroUiSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(MetroUiSettings.MySetting1));
            context.Add(
                new SettingDefinition(MetroUiSettings.FormDefaultCols,"2"),
                new SettingDefinition(MetroUiSettings.FormHorizontal,true.ToString()),
                new SettingDefinition(MetroUiSettings.FormDefaultLabelWidth,((int)LabelWidth.W100).ToString())
            );
        }
    }
}
