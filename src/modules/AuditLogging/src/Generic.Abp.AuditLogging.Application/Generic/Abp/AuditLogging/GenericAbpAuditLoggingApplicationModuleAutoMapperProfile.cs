using AutoMapper;
using Generic.Abp.AuditLogging.AuditLogs.Dtos;
using Volo.Abp.AuditLogging;

namespace Generic.Abp.AuditLogging;

public class GenericAbpAuditLoggingApplicationModuleAutoMapperProfile : Profile
{
    public GenericAbpAuditLoggingApplicationModuleAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Application layer.
        CreateMap<EntityChange, EntityChangeDto>()
            .MapExtraProperties();

        CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

        CreateMap<AuditLogAction, AuditLogActionDto>()
            .MapExtraProperties();

        CreateMap<AuditLog, AuditLogDto>()
            .MapExtraProperties();
    }
}