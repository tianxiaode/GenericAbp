using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.MyProjectName
{
    public abstract class MyProjectNameAppService : ApplicationService
    {
        protected MyProjectNameAppService()
        {
            LocalizationResource = typeof(MyProjectNameResource);
            ObjectMapperContext = typeof(GenericAbpMyProjectNameApplicationModule);
        }
    }
}
