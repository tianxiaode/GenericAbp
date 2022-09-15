using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.MyProjectName
{
    public abstract class MyProjectNameController : AbpController
    {
        protected MyProjectNameController()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
