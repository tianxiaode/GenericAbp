using Generic.Abp.ExportManager.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.ExportManager.Settings;

public class ExportManagerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        /* Define module settings here.
         * Use names from ExportManagerSettings class.
         */
        context.Add(
            new SettingDefinition(
                ExportManagerSettings.EnableExcel,
                defaultValue: "true",
                displayName: L("Display:Export.Enable.Excel"),
                isVisibleToClients: true
            )
        );

        context.Add(
            new SettingDefinition(
                ExportManagerSettings.EnableCsv,
                defaultValue: "true",
                displayName: L("Display:Export.Enable.Csv"),
                isVisibleToClients: true
            )
        );

        context.Add(
            new SettingDefinition(
                ExportManagerSettings.EnablePdf,
                defaultValue: "false",
                displayName: L("Display:Export.Enable.Pdf"),
                isVisibleToClients: true
            )
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExportManagerResource>(name);
    }
}