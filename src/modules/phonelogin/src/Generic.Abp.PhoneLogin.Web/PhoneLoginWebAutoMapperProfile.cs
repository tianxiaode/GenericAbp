using AutoMapper;
using Generic.Abp.PhoneLogin.Web.Pages.Identity.Users;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Generic.Abp.PhoneLogin.Web;

public class PhoneLoginWebAutoMapperProfile : Profile
{
    public PhoneLoginWebAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateUserMappings();
    }

    protected virtual void CreateUserMappings()
    {
        //CreateModal
        CreateMap<CreateModalModel.UserInfoViewModel, IdentityUserCreateDto>()
            .MapExtraProperties()
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, CreateModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

        //EditModal
        CreateMap<EditModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
            .MapExtraProperties()
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, EditModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
    }


}
