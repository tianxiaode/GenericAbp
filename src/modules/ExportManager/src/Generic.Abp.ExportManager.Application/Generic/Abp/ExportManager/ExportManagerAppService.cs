using Generic.Abp.ExportManager.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.ExportManager
{
    public abstract class ExportManagerAppService : ApplicationService
    {
        protected ExportManagerAppService()
        {
            LocalizationResource = typeof(ExportManagerResource);
            ObjectMapperContext = typeof(GenericAbpExportManagerApplicationModule);
        }
    }
}
