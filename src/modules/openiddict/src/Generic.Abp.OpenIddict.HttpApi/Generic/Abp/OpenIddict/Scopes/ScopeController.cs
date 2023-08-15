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

        [HttpGet]
        [Route("{id:guid}/properties")]
        public Task<List<string>> GetPropertiesAsync(Guid id)
        {
            return AppService.GetPropertiesAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/properties")]
        public Task AddPropertyAsync(Guid id, [FromBody] ScopePropertyCreateInput input)
        {
            return AppService.AddPropertyAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/properties")]
        public Task RemovePropertyAsync(Guid id, [FromBody] ScopePropertyDeleteInput input)
        {
            return AppService.RemovePropertyAsync(id, input);
        }

        [HttpGet]
        [Route("{id:guid}/resources")]
        public Task<List<string>> GetResourcesAsync(Guid id)
        {
            return AppService.GetResourcesAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/resources")]
        public Task AddResourceAsync(Guid id, [FromBody] ScopeResourceCreateInput input)
        {
            return AppService.AddResourceAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/resources")]
        public Task RemoveResourceAsync(Guid id, [FromBody] ScopeResourceDeleteInput input)
        {
            return AppService.RemoveResourceAsync(id, input);
        }
    }
}