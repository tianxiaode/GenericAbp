using AutoMapper;
using Generic.Abp.Extensions.Entities.Multilingual;
using Generic.Abp.Extensions.Entities.Permissions;
using Generic.Abp.MenuManagement.Menus;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Volo.Abp.AutoMapper;

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
                .ForMember(m => m.Parent, opts => opts.MapFrom(m => m.Parent))
                .ForMember(m => m.Multilingual, opts => opts.MapFrom(m => m.GetMultilingual()))
                .ForMember(m => m.Permissions, opts => opts.MapFrom(m => m.GetPermissions()))
                .Ignore(m => m.Children)
                .MapExtraProperties();
        }
    }
}