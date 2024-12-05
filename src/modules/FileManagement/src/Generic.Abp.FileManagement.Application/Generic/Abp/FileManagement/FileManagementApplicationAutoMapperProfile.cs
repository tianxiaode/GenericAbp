using AutoMapper;
using Generic.Abp.FileManagement.ExternalShares;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.ParticipantIsolationFolders.Dtos;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Generic.Abp.FileManagement.VirtualPaths;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

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

            CreateMap<Resource, ResourceBaseDto>()
                .ForMember(m => m.AllowedFileTypes,
                    opts => opts.MapFrom(m => m.HasConfiguration ? m.GetAllowedFileTypes() : null))
                .ForMember(m => m.UsedStorage,
                    opts => opts.MapFrom(m => m.HasConfiguration ? m.GetUsedStorage() : default!))
                .ForMember(m => m.StorageQuota,
                    opts => opts.MapFrom(m => m.HasConfiguration ? m.GetStorageQuota() : default!))
                .ForMember(m => m.MaxFileSize,
                    opts => opts.MapFrom(m => m.HasConfiguration ? m.GetMaxFileSize() : default!))
                .MapExtraProperties();

            CreateMap<ResourcePermission, ResourcePermissionDto>();

            CreateMap<VirtualPath, VirtualPathDto>().MapExtraProperties();
            CreateMap<VirtualPathGetListInput, VirtualPathQueryParams>();


            CreateMap<UserFolderGetListInput, ResourceQueryParams>()
                .Ignore(m => m.FileType);
            CreateMap<IdentityUser, UserDto>();

            CreateMap<ExternalShare, ExternalShareDto>().MapExtraProperties();
            CreateMap<ExternalShareGetListInput, ExternalShareQueryParams>();
            CreateMap<GetExternalShareResourcesDto, ResourceQueryParams>();

            CreateMap<GetParticipantIsolationFileInput, ResourceQueryParams>();
        }
    }
}