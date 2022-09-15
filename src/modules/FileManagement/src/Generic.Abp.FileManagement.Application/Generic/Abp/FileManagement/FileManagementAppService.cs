using Generic.Abp.FileManagement.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement
{
    public abstract class FileManagementAppService : ApplicationService
    {
        protected FileManagementAppService()
        {
            LocalizationResource = typeof(FileManagementResource);
            ObjectMapperContext = typeof(GenericAbpFileManagementApplicationModule);
        }
    }
}
