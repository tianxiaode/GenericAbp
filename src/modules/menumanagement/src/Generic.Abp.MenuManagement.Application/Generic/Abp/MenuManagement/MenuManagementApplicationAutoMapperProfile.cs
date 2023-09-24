using AutoMapper;
using Generic.Abp.Domain.Extensions;
using Generic.Abp.MenuManagement.Menus;
using Generic.Abp.MenuManagement.Menus.Dtos;
using System;
using Volo.Abp.Data;

namespace Generic.Abp.MenuManagement
{
    public class MenuManagementApplicationAutoMapperProfile : Profile
    {
        public MenuManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Menu, MenuDto>()
                .ForMember(m => m.Leaf, opts => opts.MapFrom(m => true))
                .ForMember(m => m.Translations,
                    opts => opts.MapFrom(m => m.GetTranslations<Menu, MenuTranslation>()))
                .ForMember(m => m.Permissions,
                    opts => opts.MapFrom(m =>
                        m.GetProperty<string>(MenuConsts.Permissions, "")
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)));


            CreateMap<MenuTranslation, MenuTranslationDto>();
            CreateMap<MenuTranslationDto, MenuTranslation>();
        }
    }
}