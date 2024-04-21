using Generic.Abp.ExportManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExportManager
{
    public abstract class ExportManagerController : AbpController
    {
        protected ExportManagerController()
        {
            LocalizationResource = typeof(ExportManagerResource);
        }
    }
}
