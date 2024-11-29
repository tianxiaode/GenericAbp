using System.Collections.Generic;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
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

        protected List<ResourceDto> MapToResourceDtos(List<Resource> resources)
        {
            return ObjectMapper.Map<List<Resource>, List<ResourceDto>>(resources);
        }
    }
}