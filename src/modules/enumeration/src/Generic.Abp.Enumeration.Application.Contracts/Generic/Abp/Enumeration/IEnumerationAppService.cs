using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Enumeration
{
    public interface IEnumerationAppService : IApplicationService
    {
        Task<ListResultDto<EnumDto>> GetEnumsAsync();
    }
}