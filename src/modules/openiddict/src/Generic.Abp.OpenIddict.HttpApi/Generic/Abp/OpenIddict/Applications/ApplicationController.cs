using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.OpenIddict.Applications
{
    [Area("OpenIddict")]
    [ControllerName("OpenIddict")]
    [Route("api/applications")]
    public class ApplicationController : OpenIddictController, IApplicationAppService
    {
        public ApplicationController(IApplicationAppService applicationAppService)
        {
            AppService = applicationAppService;
        }

        protected IApplicationAppService AppService { get; }

        [HttpGet]
        [Route("{id:guid}")]
        public Task<ApplicationDto> GetAsync(Guid id)
        {
            return AppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input)
        {
            return AppService.GetListAsync(input);
        }

        [HttpPost]
        public Task<ApplicationDto> CreateAsync([FromBody] ApplicationCreateInput input)
        {
            return AppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public Task<ApplicationDto> UpdateAsync(Guid id, [FromBody] ApplicationUpdateInput input)
        {
            return AppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public Task DeleteAsync(Guid id)
        {
            return AppService.DeleteAsync(id);
        }
    }
}