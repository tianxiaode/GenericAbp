using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Generic.Abp.ExportManager.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
