using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Generic.Abp.OpenIddict.Exceptions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

namespace Generic.Abp.OpenIddict.Applications
{
    [RemoteService(false)]
    public class ApplicationAppService : OpenIddictAppService, IApplicationAppService
    {
        public ApplicationAppService(IOpenIddictApplicationRepository openIddictApplicationRepository)
        {
            Repository = openIddictApplicationRepository;
        }

        protected IOpenIddictApplicationRepository Repository { get; }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<ApplicationDto> GetAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input)
        {
            var sorting = input.Sorting;
            if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(OpenIddictScope.Name)}";
            var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
            var count = await Repository.GetCountAsync();
            return new PagedResultDto<ApplicationDto>(count,
                ObjectMapper.Map<List<OpenIddictApplication>, List<ApplicationDto>>(list));
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Create)]
        public virtual async Task<ApplicationDto> CreateAsync(ApplicationCreateInput input)
        {
            var entity = new OpenIddictApplication(GuidGenerator.Create());
            await UpdateByInputAsync(entity, input);

            await Repository.InsertAsync(entity);

            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);

        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input)
        {
            var entity = await Repository.GetAsync(id);
            entity.ConcurrencyStamp = input.ConcurrencyStamp;
            await UpdateByInputAsync(entity, input);
            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);

        }

        [UnitOfWork]
        protected virtual async Task UpdateByInputAsync(OpenIddictApplication entity, ApplicationCreateOrUpdateInput input)
        {
            var exits = await Repository.FindByClientIdAsync(input.ClientId);
            if (exits != null && (entity.Id != exits.Id))
            {
                throw new DuplicateWarningBusinessException(nameof(OpenIddictScope.Name), input.ClientId);
            }
            if (!OpenIddictConstants.ClientTypes.Confidential.Equals(input.Type) && !OpenIddictConstants.ClientTypes.Public.Equals(input.Type))
            {
                throw new ClientTypeErrorBusinessException();
            }
            if (!OpenIddictConstants.ConsentTypes.Implicit.Equals(input.ConsentType) && !OpenIddictConstants.ConsentTypes.Systematic.Equals(input.ConsentType)
                && !OpenIddictConstants.ConsentTypes.External.Equals(input.ConsentType) && !OpenIddictConstants.ConsentTypes.Explicit.Equals(input.ConsentType))
            {
                throw new ConsentTypeErrorBusinessException();
            }
            entity.DisplayName = input.DisplayName;
            entity.ClientSecret = input.ClientSecret;
            entity.ConsentType = input.ConsentType;
            entity.ClientUri = input.ClientUri;
            entity.LogoUri = input.LogoUri;
            entity.Type = input.Type;
            if (!input.Permissions.IsNullOrEmpty())
            {
                entity.Permissions = System.Text.Json.JsonSerializer.Serialize(input.Permissions);
            }
            if (!input.PostLogoutRedirectUris.IsNullOrEmpty())
            {
                entity.PostLogoutRedirectUris = System.Text.Json.JsonSerializer.Serialize(input.PostLogoutRedirectUris);
            }
            if (!input.Properties.IsNullOrEmpty())
            {
                entity.Properties = System.Text.Json.JsonSerializer.Serialize(input.Properties);
            }
            if (!input.RedirectUris.IsNullOrEmpty())
            {
                entity.RedirectUris = System.Text.Json.JsonSerializer.Serialize(input.RedirectUris);
            }
            if (!input.Requirements.IsNullOrEmpty())
            {
                entity.Requirements = System.Text.Json.JsonSerializer.Serialize(input.Requirements);
            }
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Delete)]
        public virtual async Task<ListResultDto<ApplicationDto>> DeleteAsync(List<Guid> ids)
        {
            var result = new List<ApplicationDto>();
            foreach (var guid in ids)
            {
                var entity = await Repository.FindAsync(guid);
                if (entity == null) continue;
                await Repository.DeleteAsync(entity);
                result.Add(ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity));
            }



            return new ListResultDto<ApplicationDto>(result);
        }


    }
}
