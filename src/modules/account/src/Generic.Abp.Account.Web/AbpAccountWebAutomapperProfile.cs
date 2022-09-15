using AutoMapper;
using Generic.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace Generic.Abp.Account.Web
{
    public class AbpAccountWebAutoMapperProfile : Profile
    {
        public AbpAccountWebAutoMapperProfile()
        {
            CreateMap<ProfileDto, PersonalSettingsInfoModel>();
        }
    }
}
