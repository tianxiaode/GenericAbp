using System;
using AutoMapper;
using Generic.Abp.Domain.Extensions;
using Generic.Abp.Identity.Roles;
using Generic.Abp.Identity.Users;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity
{
    public class IdentityApplicationAutoMapperProfile : Profile
    {
        public IdentityApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<IdentityRole, RoleDto>()
                .ForMember(m => m.Permissions, opts => opts.Ignore())
                .ForMember(m => m.Translations,
                    opts => opts.MapFrom(m => m.GetTranslations<IdentityRole, RoleTranslation>()))
                .ForMember(m=>m.ExtraProperties, opts=>opts.Ignore());

            CreateMap<RoleTranslation, RoleTranslationDto>();

            CreateMap<IdentityRole, UserGetRoleDto>()
                .Ignore(m=>m.IsSelected)
                .Ignore(m=>m.Permissions)
                .Ignore(m=>m.Translations);

            CreateMap<Tuple<IdentityRole, bool>, UserGetRoleDto>()
                .ForMember(m => m.IsSelected, opts => opts.MapFrom(m => m.Item2))
                .ForMember(m => m.Id, opts => opts.MapFrom(m => m.Item1.Id))
                .ForMember(m => m.Name, opts => opts.MapFrom(m => m.Item1.Name))
                .ForMember(m => m.IsDefault, opts => opts.MapFrom(m => m.Item1.IsDefault))
                .ForMember(m => m.IsPublic, opts => opts.MapFrom(m => m.Item1.IsPublic))
                .ForMember(m => m.IsStatic, opts => opts.MapFrom(m => m.Item1.IsStatic))
                .ForMember(m => m.ConcurrencyStamp, opts => opts.MapFrom(m => m.Item1.ConcurrencyStamp))
                .ForMember(m => m.Permissions, opts => opts.Ignore())
                .ForMember(m => m.Translations,
                    opts => opts.MapFrom(m => m.Item1.GetTranslations<IdentityRole, RoleTranslation>()))
                .ForMember(m=>m.ExtraProperties, opts=>opts.Ignore());

        }
    }
}