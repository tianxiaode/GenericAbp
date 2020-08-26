using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Enumeration
{
    [Area("configuration")]
    [RemoteService(Name = "configuration")]
    [ControllerName("Configuration")]
    [Route("api/configuration")]
    public class EnumerationController : AbpController, IEnumerationAppService
    {
        private readonly IEnumerationAppService _enumerationAppService;

        public EnumerationController(IEnumerationAppService enumerationAppService)
        {
            _enumerationAppService = enumerationAppService;
        }

        [HttpGet]
        [Route("enums")]
        [Authorize]
        public virtual async Task<ListResultDto<EnumDto>> GetEnumsAsync()
        {
            return await _enumerationAppService.GetEnumsAsync();
        }
    }
}