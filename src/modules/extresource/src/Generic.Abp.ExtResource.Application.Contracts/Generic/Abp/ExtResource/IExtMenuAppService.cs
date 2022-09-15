using System.Threading.Tasks;
using Generic.Abp.ExtResource.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.ExtResource
{
    public interface IExtMenuAppService : IApplicationService
    {
        Task<ListResultDto<DesktopMenuItemDto>> GetDesktopMenusAsync();
        Task<ListResultDto<PhoneMenuItemDto>> GetPhoneMenusAsync();
    }
}
