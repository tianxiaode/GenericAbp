using AutoMapper;
using Generic.Abp.Account.Identity.Web.Pages.Identity.Roles;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using CreateUserModalModel = Generic.Abp.Account.Identity.Web.Pages.Identity.Users.CreateModalModel;
using EditUserModalModel = Generic.Abp.Account.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace Generic.Abp.Account.Identity.Web
{
    public class AbpIdentityWebAutoMapperProfile : Profile
    {
        public AbpIdentityWebAutoMapperProfile()
        {
            CreateUserMappings();
            CreateRoleMappings();
        }

        protected virtual void CreateUserMappings()
        {
            //List
            CreateMap<IdentityUserDto, EditUserModalModel.UserInfoViewModel>()
                .Ignore(x => x.Password)
                .Ignore(x=>x.TwoFactorEnabled);

            //CreateModal
            CreateMap<CreateUserModalModel.UserInfoViewModel, IdentityUserCreateDto>()
                .Ignore(x => x.ExtraProperties)
                .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, CreateUserModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

            //EditModal
            CreateMap<EditUserModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
                .Ignore(x => x.ExtraProperties)
                .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, EditUserModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
        }

        protected virtual void CreateRoleMappings()
        {
            //List
            CreateMap<IdentityRoleDto, EditModalModel.RoleInfoModel>();

            //CreateModal
            CreateMap<CreateModalModel.RoleInfoModel, IdentityRoleCreateDto>()
                .Ignore(x => x.ExtraProperties);

            //EditModal
            CreateMap<EditModalModel.RoleInfoModel, IdentityRoleUpdateDto>()
                .Ignore(x => x.ExtraProperties);
        }
    }
}
