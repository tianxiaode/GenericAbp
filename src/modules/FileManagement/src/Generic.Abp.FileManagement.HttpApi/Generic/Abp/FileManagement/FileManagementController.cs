using Generic.Abp.FileManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.FileManagement;

public abstract class FileManagementController : AbpController
{
    protected FileManagementController()
    {
        LocalizationResource = typeof(FileManagementResource);
    }
}