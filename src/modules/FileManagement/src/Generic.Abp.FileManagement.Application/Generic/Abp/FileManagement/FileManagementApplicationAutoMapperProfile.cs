using AutoMapper;
using Generic.Abp.FileManagement.Files;

namespace Generic.Abp.FileManagement
{
    public class FileManagementApplicationAutoMapperProfile : Profile
    {
        public FileManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Files.File, FileDto>();

            CreateMap<FileCheckResult, FileCheckResultDto>();
        }
    }
}