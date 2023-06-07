using AutoMapper;
using Generic.Abp.Metro.UI.PermissionManagement.Web.Pages.AbpPermissionManagement;
using Volo.Abp.AutoMapper;
using Volo.Abp.PermissionManagement;

namespace Generic.Abp.Metro.UI.PermissionManagement.Web;

public class AbpPermissionManagementWebAutoMapperProfile : Profile
{
    public AbpPermissionManagementWebAutoMapperProfile()
    {
        CreateMap<PermissionGroupDto, PermissionManagementModal.PermissionGroupViewModel>()
            .Ignore(p => p.IsAllPermissionsGranted);

        CreateMap<PermissionGrantInfoDto, PermissionManagementModal.PermissionGrantInfoViewModel>()
            .ForMember(p => p.Depth, opts => opts.Ignore());

        CreateMap<ProviderInfoDto, PermissionManagementModal.ProviderInfoViewModel>();
    }
}