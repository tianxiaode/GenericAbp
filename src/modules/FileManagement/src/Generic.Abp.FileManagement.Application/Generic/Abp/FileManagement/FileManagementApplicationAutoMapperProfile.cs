using AutoMapper;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.Folders.Dtos;
using Generic.Abp.FileManagement.VirtualPaths;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;

namespace Generic.Abp.FileManagement
{
    public class FileManagementApplicationAutoMapperProfile : Profile
    {
        public FileManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Files.File, FileDto>()
                .ForMember(m => m.MimeType, opts => opts.MapFrom(m => m.FileInfoBase.MimeType))
                .ForMember(m => m.FileType, opts => opts.MapFrom(m => m.FileInfoBase.FileType))
                .ForMember(m => m.Size, opts => opts.MapFrom(m => m.FileInfoBase.Size))
                .MapExtraProperties();

            CreateMap<FileCheckResult, FileCheckResultDto>();


            CreateMap<Folder, FolderDto>()
                .ForMember(m => m.Leaf, opts => opts.MapFrom(m => true))
                .ForMember(m => m.Parent, opts => opts.MapFrom(m => m.Parent))
                .MapExtraProperties();

            CreateMap<VirtualPath, VirtualPathDto>()
                .MapExtraProperties();

            CreateMap<VirtualPathPermission, VirtualPathPermissionDto>();
            CreateMap<FilePermission, PermissionDto>();
            CreateMap<FolderPermission, PermissionDto>();

            // CreateMap<VirtualPathPermissionCreateOrUpdateDto, VirtualPathPermission>()
            //     .ForMember(dest => dest.TargetId, opt => opt.MapAtRuntime());

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PermissionCreateOrUpdateDto, VirtualPathPermission>()
                    .ForMember(dest => dest.TargetId, opt => opt.MapFrom((src, dest, destMember, context) =>
                    {
                        // 获取映射上下文中的参数
                        var targetId = (string)context.Items["TargetId"];
                        return targetId;
                    }));
                cfg.CreateMap<PermissionCreateOrUpdateDto, FilePermission>()
                    .ForMember(dest => dest.TargetId, opt => opt.MapFrom((src, dest, destMember, context) =>
                    {
                        // 获取映射上下文中的参数
                        var targetId = (string)context.Items["TargetId"];
                        return targetId;
                    }));
                cfg.CreateMap<PermissionCreateOrUpdateDto, FolderPermission>()
                    .ForMember(dest => dest.TargetId, opt => opt.MapFrom((src, dest, destMember, context) =>
                    {
                        // 获取映射上下文中的参数
                        var targetId = (string)context.Items["TargetId"];
                        return targetId;
                    }));
            });
        }
    }
}