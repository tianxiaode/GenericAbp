using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.MenuManagement
{
    public abstract class MenuManagementController : AbpController
    {
        protected MenuManagementController()
        {
            LocalizationResource = typeof(MenuManagementResource);
        }
    }
}