using AutoMapper;
using Generic.Abp.Identity.SecurityLogs.Dtos;
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
            CreateMap<IdentitySecurityLog, SecurityLogDto>()
                .MapExtraProperties();
        }
    }
}