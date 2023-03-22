using AutoMapper;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace Generic.Abp.Account;

public class AbpAccountApplicationModuleAutoMapperProfile : Profile
{
    public AbpAccountApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, ProfileDto>()
            .ForMember(dest => dest.HasPassword,
                op => op.MapFrom(src => src.PasswordHash != null))
            .MapExtraProperties();
    }
}