using AutoMapper;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.Folders.Dtos;

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

            CreateMap<Folder, FolderDto>()
                .ForMember(m => m.Leaf, opts => opts.MapFrom(m => true))
                .ForMember(m => m.Parent, opts => opts.MapFrom(m => m.Parent))
                .MapExtraProperties();
        }
    }
}