using AutoMapper;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Generic.Abp.FileManagement.VirtualPaths;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Volo.Abp.AutoMapper;

namespace Generic.Abp.FileManagement
{
    public class FileManagementApplicationAutoMapperProfile : Profile
    {
        public FileManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */


            CreateMap<FileCheckResult, FileCheckResultDto>();

            CreateMap<Resource, ResourceBaseDto>();

            CreateMap<ResourcePermission, ResourcePermissionDto>();

            CreateMap<VirtualPath, VirtualPathDto>().MapExtraProperties();
            CreateMap<VirtualPathGetListInput, VirtualPathSearchParams>();

            CreateMap<Resource, UserFolderDto>()
                .ForMember(m => m.AllowedFileTypes, opts => opts.MapFrom(m => m.Configuration!.AllowedFileTypes))
                .ForMember(m => m.UsedStorage, opts => opts.MapFrom(m => m.Configuration!.UsedStorage))
                .ForMember(m => m.StorageQuota, opts => opts.MapFrom(m => m.Configuration!.StorageQuota))
                .ForMember(m => m.MaxFileSize, opts => opts.MapFrom(m => m.Configuration!.MaxFileSize));

            CreateMap<UserFolderGetListInput, ResourceSearchAndPagedAndSortedParams>()
                .Ignore(m => m.FileType);
        }
    }
}