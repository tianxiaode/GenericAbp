using AutoMapper;
using Generic.Abp.Metro.UI.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Volo.Abp.Account;

namespace Generic.Abp.Metro.UI.Account.Web;

public class AbpAccountWebAutoMapperProfile : Profile
{
    public AbpAccountWebAutoMapperProfile()
    {
        CreateMap<ProfileDto, AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel>()
            .MapExtraProperties();
    }
}