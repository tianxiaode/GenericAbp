using Generic.Abp.Identity.SecurityLogs.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.SecurityLogs;

[RemoteService(false)]
public class SecurityLogAppService : IdentityAppService, ISecurityLogAppService
{
    protected ISecurityLogRepository SecurityLogRepository { get; }

    public SecurityLogAppService(ISecurityLogRepository securityLogRepository)
    {
        SecurityLogRepository = securityLogRepository;
    }

    public virtual async Task<SecurityLogDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<IdentitySecurityLog, SecurityLogDto>(await SecurityLogRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input)
    {
        var predicate = await SecurityLogRepository.BuildPredicateAsync(input.Filter, input.StartTime, input.EndTime,
            input.ApplicationName, input.Identity, input.ActionName, null, input.UserName, input.ClientId,
            input.CorrelationId);
        var totalCount = await SecurityLogRepository.GetCountAsync(predicate);
        if (totalCount == 0)
        {
            return new PagedResultDto<SecurityLogDto>(0, new List<SecurityLogDto>());
        }

        var securityLogs =
            await SecurityLogRepository.GetListAsync(predicate, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<SecurityLogDto>(totalCount,
            ObjectMapper.Map<List<IdentitySecurityLog>, List<SecurityLogDto>>(securityLogs));
    }

    public virtual async Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter)
    {
        return new ListResultDto<string>((await SecurityLogRepository.GetAllApplicationNamesAsync())
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public virtual async Task<ListResultDto<string>> GetAllIdentitiesAsync()
    {
        return new ListResultDto<string>(await SecurityLogRepository.GetAllIdentitiesAsync());
    }

    public virtual async Task<ListResultDto<string>> GetAllActionsAsync()
    {
        return new ListResultDto<string>(await SecurityLogRepository.GetAllActionsAsync());
    }

    public virtual async Task<ListResultDto<string>> GetAllUserNamesAsync()
    {
        return new ListResultDto<string>(await SecurityLogRepository.GetAllUserNamesAsync());
    }

    public virtual async Task<ListResultDto<string>> GetAllClientIdsAsync()
    {
        return new ListResultDto<string>(await SecurityLogRepository.GetAllClientIdsAsync());
    }

    public virtual async Task<ListResultDto<string>> GetAllCorrelationIdsAsync()
    {
        return new ListResultDto<string>(await SecurityLogRepository.GetAllCorrelationIdsAsync());
    }
}