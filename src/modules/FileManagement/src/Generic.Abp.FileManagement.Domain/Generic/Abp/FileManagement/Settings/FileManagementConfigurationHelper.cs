using System.Threading.Tasks;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.Settings;

public static class FileManagementConfigurationHelper
{
    public static async Task<bool> GetEnableAsync(
        ISettingManager settingManager,
        string settingName)
    {
        var value = await settingManager.GetOrNullForCurrentTenantAsync(settingName);
        return bool.TryParse(value, out var enable) && enable;
    }

    public static async Task<int> GetRetentionPeriodAsync(
        ISettingManager settingManager,
        string settingName,
        int defaultValue)
    {
        var value = await settingManager.GetOrNullForCurrentTenantAsync(settingName);
        return int.TryParse(value, out var period) ? period : defaultValue;
    }

    public static async Task<int> GetFrequencyAsync(
        ISettingManager settingManager,
        string settingName,
        int defaultValue)
    {
        var value = await settingManager.GetOrNullForCurrentTenantAsync(settingName);
        return int.TryParse(value, out var frequency) ? frequency : defaultValue;
    }

    public static async Task<int> GetBatchSizeAsync(
        ISettingManager settingManager,
        string settingName,
        int defaultValue)
    {
        var value = await settingManager.GetOrNullForCurrentTenantAsync(settingName);
        return int.TryParse(value, out var batchSize) ? batchSize : defaultValue;
    }
}