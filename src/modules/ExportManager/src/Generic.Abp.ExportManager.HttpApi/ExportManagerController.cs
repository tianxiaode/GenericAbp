using Generic.Abp.ExportManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExportManager;

public abstract class ExportManagerController : AbpControllerBase
{
    protected ExportManagerController()
    {
        LocalizationResource = typeof(ExportManagerResource);
    }
}
