using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.ExtMenu
{
    public interface IExtMenuAppService : IApplicationService
    {
        Task<ListResultDto<DesktopMenuItemDto>> GetDesktopMenusAsync();
        Task<ListResultDto<PhoneMenuItemDto>> GetPhoneMenusAsync();
    }
}
