using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.OpenIddict.Scopes
{
    [Area("OpenIddict")]
    [ControllerName("OpenIddict")]
    [Route("api/scopes")]

    public class ScopeController : OpenIddictController, IScopeAppService
    {
        public ScopeController(IScopeAppService scopeAppService)
        {
            AppService = scopeAppService;
        }
        protected IScopeAppService AppService { get; }

        [HttpGet]
        [Route("{id:guid}")]
        public Task<ScopeDto> GetAsync(Guid id)
        {
            return AppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<ScopeDto>> GetListAsync(ScopeGetListInput input)
        {
            return AppService.GetListAsync(input);
        }

        [HttpPost]
        public Task<ScopeDto> CreateAsync([FromBody] ScopeCreateInput input)
        {
            return AppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public Task<ScopeDto> UpdateAsync(Guid id, [FromBody] ScopeUpdateInput input)
        {
            return AppService.UpdateAsync(id, input);
        }
        [HttpDelete]
        public Task<ListResultDto<ScopeDto>> DeleteAsync([FromBody] List<Guid> ids)
        {
            return AppService.DeleteAsync(ids);
        }

    }
}
