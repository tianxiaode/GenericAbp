using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public Task<ListResultDto<ApplicationDto>> DeleteAsync([FromBody] List<Guid> ids)
        {
            return AppService.DeleteAsync(ids);
        }

        [HttpGet]
        [Route("permissions")]
        public Task<Dictionary<string, Dictionary<string, string>>> GetAllPermisions()
        {
            return AppService.GetAllPermisions();
        }

        [HttpGet]
        [Route("{id:guid}/permissions")]
        public Task<List<string>> GetPermissionsAsync(Guid id)
        {
            return AppService.GetPermissionsAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/permissions")]
        public Task AddPermissionAsync(Guid id, [FromBody] PermissionCreateInput input)
        {
            return AppService.AddPermissionAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/permissions")]
        public Task RemovePermissionAsync(Guid id, [FromBody] PermissionDeleteInput input)
        {
            return AppService.RemovePermissionAsync(id, input);
        }

        [HttpGet]
        [Route("{id:guid}/post-logout-redirect-uris")]
        public Task<List<string>> GetPostLogoutRedirectUrisAsync(Guid id)
        {
            return AppService.GetPostLogoutRedirectUrisAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/post-logout-redirect-uris")]
        public Task AddPostLogoutRedirectUriAsync(Guid id, [FromBody] PostLogoutRedirectUriCreateInput input)
        {
            return AppService.AddPostLogoutRedirectUriAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/post-logout-redirect-uris")]
        public Task RemovePostLogoutRedirectUriAsync(Guid id,
            [FromBody] PostLogoutRedirectUriDeleteInput input)
        {
            return AppService.RemovePostLogoutRedirectUriAsync(id, input);
        }

        [HttpGet]
        [Route("{id:guid}/properties")]
        public Task<List<string>> GetPropertiesAsync(Guid id)
        {
            return AppService.GetPropertiesAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/properties")]
        public Task AddPropertyAsync(Guid id, [FromBody] PropertyCreateInput input)
        {
            return AppService.AddPropertyAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/properties")]
        public Task RemovePropertyAsync(Guid id, [FromBody] PropertyDeleteInput input)
        {
            return AppService.RemovePropertyAsync(id, input);
        }

        [HttpGet]
        [Route("{id:guid}/redirect-uris")]
        public Task<List<string>> GetRedirectUrisAsync(Guid id)
        {
            return AppService.GetRedirectUrisAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/redirect-uris")]
        public Task AddRedirectUriAsync(Guid id, [FromBody] RedirectUriCreateInput input)
        {
            return AppService.AddRedirectUriAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/redirect-uris")]
        public Task RemoveRedirectUriAsync(Guid id, [FromBody] RedirectUriDeleteInput input)
        {
            return AppService.RemoveRedirectUriAsync(id, input);
        }

        [HttpGet]
        [Route("{id:guid}/requirements")]
        public Task<List<string>> GetRequirementsAsync(Guid id)
        {
            return AppService.GetRequirementsAsync(id);
        }

        [HttpPut]
        [Route("{id:guid}/requirements")]
        public Task AddRequirementAsync(Guid id, [FromBody] RequirementCreateInput input)
        {
            return AppService.AddRequirementAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}/requirements")]
        public Task RemoveRequirementAsync(Guid id, [FromBody] RequirementDeleteInput input)
        {
            return AppService.RemoveRequirementAsync(id, input);
        }
    }
}