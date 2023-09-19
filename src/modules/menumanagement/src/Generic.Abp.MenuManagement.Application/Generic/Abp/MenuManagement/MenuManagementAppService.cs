using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.MenuManagement
{
    public abstract class MenuManagementAppService : ApplicationService
    {
        protected MenuManagementAppService()
        {
            LocalizationResource = typeof(MenuManagementResource);
            ObjectMapperContext = typeof(GenericAbpMenuManagementApplicationModule);
        }
    }
}
