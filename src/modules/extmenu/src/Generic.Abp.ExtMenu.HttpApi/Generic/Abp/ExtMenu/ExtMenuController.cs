using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExtMenu
{
    [Area("configuration")]
    [RemoteService(Name = "configuration")]
    [Route("api/configuration/")]
    public class ExtMenuController : AbpController, IExtMenuAppService
    {
        private readonly IExtMenuAppService _extMenuAppService;

        public ExtMenuController(IExtMenuAppService extMenuAppService)
        {
            _extMenuAppService = extMenuAppService;
        }

        [HttpGet]
        [Route("menus/desktop")]
        public Task<ListResultDto<DesktopMenuItemDto>> GetDesktopMenusAsync()
        {
            return _extMenuAppService.GetDesktopMenusAsync();
        }

        [HttpGet]
        [Route("menus/phone")]
        public Task<ListResultDto<PhoneMenuItemDto>> GetPhoneMenusAsync()
        {
            return _extMenuAppService.GetPhoneMenusAsync();
        }
    }
}
