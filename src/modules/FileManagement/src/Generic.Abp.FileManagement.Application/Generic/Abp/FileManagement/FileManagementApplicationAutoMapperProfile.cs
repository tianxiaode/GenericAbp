using AutoMapper;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
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


            CreateMap<Resource, ResourceDto>()
                .ForMember(m => m.AllowedFileTypes, opts => opts.MapFrom(m => m.GetAllowedFileTypes()))
                .ForMember(m => m.FileMaxSize, opts => opts.MapFrom(m => m.GetMaxFileSize()))
                .ForMember(m => m.Used, opts => opts.MapFrom(m => m.GetUsedSize()))
                .ForMember(m => m.Quota, opts => opts.MapFrom(m => m.GetQuota()))
                .ForMember(m => m.MimeType,
                    opts => opts.MapFrom(m => m.FileInfoBase == null ? "" : m.FileInfoBase.Hash))
                .ForMember(m => m.Hash,
                    opts => opts.MapFrom(m => m.FileInfoBase == null ? "" : m.FileInfoBase.MimeType))
                .ForMember(m => m.Extension,
                    opts => opts.MapFrom(m => m.FileInfoBase == null ? "" : m.FileInfoBase.Extension))
                .ForMember(m => m.Size, opts => opts.MapFrom(m => m.FileInfoBase == null ? 0 : m.FileInfoBase.Size))
                .Ignore(m => m.Leaf);

            CreateMap<ResourcePermission, ResourcePermissionDto>();
        }
    }
}