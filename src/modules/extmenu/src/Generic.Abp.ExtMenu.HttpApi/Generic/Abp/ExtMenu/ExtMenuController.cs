using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExtMenu
{
    [Area("configuration")]
    // [RemoteService(Name = "configuration")]
    [ControllerName("Configuration")]
    [Route("api/configuration/")]
    [RemoteService]
    public class ExtMenuController : AbpController, IExtMenuAppService
    {
        private readonly IExtMenuAppService _extMenuAppService;

        public ExtMenuController(IExtMenuAppService extMenuAppService)
        {
            _extMenuAppService = extMenuAppService;
        }

        [HttpGet]
        [Route("menus")]
        public async Task<List<IMenuItemBaseDto>> GetMenusAsync(bool isPhone = false)
        {
            return await _extMenuAppService.GetMenusAsync(isPhone);
        }


    }
}
