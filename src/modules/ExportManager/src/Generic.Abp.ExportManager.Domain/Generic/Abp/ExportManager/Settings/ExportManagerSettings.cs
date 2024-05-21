namespace Generic.Abp.ExportManager.Settings
{
    public static class ExportManagerSettings
    {
        public const string GroupName = "ExportManager";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        public const string EnableExcel = GroupName + ".EnableExcel";
        public const string EnableCsv = GroupName + ".EnableCsv";
        public const string EnablePdf = GroupName + ".EnablePdf";
    }
}