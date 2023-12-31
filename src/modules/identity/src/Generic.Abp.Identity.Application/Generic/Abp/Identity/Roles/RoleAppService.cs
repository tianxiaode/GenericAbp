using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.Domain.Entities;
using Generic.Abp.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace Generic.Abp.Identity.Roles;

[RemoteService(false)]
[Authorize(IdentityPermissions.Roles.Default)]
public class RoleAppService : IdentityAppService, IRoleAppService
{
    public RoleAppService(IdentityRoleManager roleManager, IIdentityRoleRepository roleRepository,
        IPermissionManager permissionManager, IPermissionDefinitionManager permissionDefinitionManager)
    {
        RoleManager = roleManager;
        RoleRepository = roleRepository;
        PermissionManager = permissionManager;
        PermissionDefinitionManager = permissionDefinitionManager;
    }

    protected IdentityRoleManager RoleManager { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    public virtual async Task<RoleDto> GetAsync(Guid id)
    {
        var role = await RoleManager.GetByIdAsync(id);
        var dto = ObjectMapper.Map<IdentityRole, RoleDto>(role);
        dto.Permissions = (await PermissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, dto.Name))
            .Where(m => m.IsGranted)
            .Select(m => m.Name).OrderBy(m => m).ToList();
        return dto;
    }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    public virtual async Task<ListResultDto<RoleDto>> GetAllListAsync()
    {
        var list = await RoleRepository.GetListAsync();
        return new ListResultDto<RoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(list)
        );
    }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    public virtual async Task<PagedResultDto<RoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        var list = await RoleRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
            input.Filter);
        var totalCount = await RoleRepository.GetCountAsync(input.Filter);

        var dtos = ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(list);
        foreach (var dto in dtos)
        {
            dto.Permissions =
                (await PermissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, dto.Name))
                .Where(m => m.IsGranted)
                .Select(m => m.Name).OrderBy(m => m).ToList();
        }

        return new PagedResultDto<RoleDto>(totalCount, dtos);
    }

    [Authorize(IdentityPermissions.Roles.Create)]
    [UnitOfWork]
    public virtual async Task<RoleDto> CreateAsync(RoleCreateDto input)
    {
        var role = new IdentityRole(
            GuidGenerator.Create(),
            input.Name,
            CurrentTenant.Id
        )
        {
            IsDefault = input.IsDefault,
            IsPublic = input.IsPublic
        };

        input.MapExtraPropertiesTo(role);

        (await RoleManager.CreateAsync(role)).CheckErrors();

        var permissions = await NormalizedPermissionsAsync(input);

        foreach (var permission in permissions)
        {
            await PermissionManager.SetAsync(permission, RolePermissionValueProvider.ProviderName, input.Name,
                true);
        }

        await CurrentUnitOfWork.SaveChangesAsync();
        var dto = ObjectMapper.Map<IdentityRole, RoleDto>(role);
        dto.Permissions = input.Permissions;
        return dto;
    }

    [Authorize(IdentityPermissions.Roles.Update)]
    [UnitOfWork]
    public virtual async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto input)
    {
        var role = await RoleManager.GetByIdAsync(id);
        if (role.Name.Equals("admin", StringComparison.OrdinalIgnoreCase) && CurrentUser?.UserName != "admin")
        {
            throw new UserFriendlyException("Role admin is not allowed to change");
        }

        role.ConcurrencyStamp = input.ConcurrencyStamp;
        var oldName = role.Name;

        (await RoleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();

        role.IsDefault = input.IsDefault;
        role.IsPublic = input.IsPublic;

        input.MapExtraPropertiesTo(role);

        (await RoleManager.UpdateAsync(role)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        var permissions = await NormalizedPermissionsAsync(input);

        var rolePermissions =
            (await PermissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, oldName))
            .Where(m => m.IsGranted).Select(m => m.Name).ToList();

        //删除已取消的权限
        foreach (var permission in rolePermissions.Except(permissions))
        {
            await PermissionManager.SetAsync(permission, RolePermissionValueProvider.ProviderName, oldName, false);
        }

        //添加新权限
        foreach (var permission in permissions.Except(rolePermissions))
        {
            await PermissionManager.SetAsync(permission, RolePermissionValueProvider.ProviderName, oldName, true);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var dto = ObjectMapper.Map<IdentityRole, RoleDto>(role);
        dto.Permissions = input.Permissions;
        return dto;
    }

    [UnitOfWork]
    protected virtual Task<List<string>> NormalizedPermissionsAsync(RoleCreateOrUpdateDtoBase input)
    {
        var result = new List<string>();
        result.AddRange(input.Permissions);
        var adds = from permission in input.Permissions
            select permission.Split(".", StringSplitOptions.RemoveEmptyEntries)
            into split
            where split.Length >= 3
            select $"{split[0]}.{split[1]}.ManagePermissions";
        result.AddRange(adds);
        return Task.FromResult(result);
    }


    [Authorize(IdentityPermissions.Roles.Delete)]
    [UnitOfWork]
    public virtual async Task<ListResultDto<RoleDto>> DeleteAsync(List<Guid> ids)
    {
        var result = new List<RoleDto>();
        foreach (var id in ids)
        {
            var role = await RoleManager.GetByIdAsync(id);
            if (role.Name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new UserFriendlyException("Role admin is not allowed to change");
            }

            if (role.IsStatic)
            {
                throw new EntityNotBeDeletedBusinessException(L["Role"], role.Name);
            }

            (await RoleManager.DeleteAsync(role)).CheckErrors();
            result.Add((RoleDto)ObjectMapper.Map<IdentityRole, RoleDto>(role));

            var rolePermissions =
                (await PermissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, role.Name))
                .Where(m => m.IsGranted).Select(m => m.Name).ToList();

            foreach (var permission in rolePermissions)
            {
                await PermissionManager.SetAsync(permission, RolePermissionValueProvider.ProviderName, role.Name,
                    false);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        return new ListResultDto<RoleDto>(result);
    }

    [Authorize(IdentityPermissions.Roles.Update)]
    [UnitOfWork]
    public virtual async Task SetDefaultAsync(Guid id, bool value)
    {
        var entity = await RoleRepository.GetAsync(id);
        entity.IsDefault = value;
        await RoleRepository.UpdateAsync(entity);
    }

    [Authorize(IdentityPermissions.Roles.Update)]
    [UnitOfWork]
    public virtual async Task SetPublicAsync(Guid id, bool value)
    {
        var entity = await RoleRepository.GetAsync(id);
        entity.IsPublic = value;
        await RoleRepository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    public virtual async Task<ListResultDto<RoleTranslationDto>> GetTranslationAsync(Guid id)
    {
        var entity = await RoleRepository.GetAsync(id);
        var translations = entity.GetTranslations<IdentityRole, RoleTranslation>();

        return new ListResultDto<RoleTranslationDto>(translations.Select(m =>
            new RoleTranslationDto(m.Language, m.Name)).ToList());
    }

    [Authorize(IdentityPermissions.Roles.Update)]
    [UnitOfWork]
    public virtual async Task UpdateTranslationAsync(Guid id, List<RoleTranslationDto> translations)
    {
        translations = translations.Where(m => !string.IsNullOrEmpty(m.Name)).ToList();
        await CheckTranslationInputAsync(translations);
        var entity = await RoleRepository.GetAsync(id);
        entity.SetTranslations(translations.Select(m => new RoleTranslation(m.Language, m.Name)));

        await CurrentUnitOfWork.SaveChangesAsync();
    }


    protected virtual Task CheckTranslationInputAsync(List<RoleTranslationDto> translations)
    {
        var errors = translations.Where(m => m.Name.Length > IdentityRoleConsts.MaxNameLength).Select(m =>
            L[BusinessExceptionErrorCodes.ValueExceedsFieldLength].Value
                .Replace(BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamValue, m.Name).Replace(
                    BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamLength,
                    DistrictConsts.DisplayNameMaxLength.ToString())).ToList();

        if (errors.Any())
        {
            throw new UserFriendlyException(string.Join("<br>", errors));
        }

        return Task.CompletedTask;
    }
}