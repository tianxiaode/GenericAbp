using AutoMapper;
using Generic.Abp.FileManagement.ExternalShares;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
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
            CreateMap<VirtualPathGetListInput, VirtualPathQueryParams>();

            CreateMap<Resource, UserFolderDto>()
                .ForMember(m => m.AllowedFileTypes, opts => opts.MapFrom(m => m.GetAllowedFileTypes()))
                .ForMember(m => m.UsedStorage, opts => opts.MapFrom(m => m.GetUsedStorage()))
                .ForMember(m => m.StorageQuota, opts => opts.MapFrom(m => m.GetStorageQuota()))
                .ForMember(m => m.MaxFileSize, opts => opts.MapFrom(m => m.GetMaxFileSize()));

            CreateMap<UserFolderGetListInput, ResourceSearchParams>()
                .Ignore(m => m.FileType);

            CreateMap<ExternalShare, ExternalShareDto>().MapExtraProperties();
            CreateMap<ExternalShareGetListInput, ExternalShareSearchParams>();
        }
    }
}