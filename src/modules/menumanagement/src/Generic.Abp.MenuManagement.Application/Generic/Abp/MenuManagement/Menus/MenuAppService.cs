using Generic.Abp.Extensions.Entities.Multilingual;
using Generic.Abp.Extensions.Entities.Permissions;
using Generic.Abp.Extensions.Entities.Trees;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Validates;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Generic.Abp.MenuManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuAppService(
    MenuManager menuManager,
    IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
    IOptions<AbpLocalizationOptions> options,
    IPermissionDefinitionManager permissionDefinitionManager)
    : MenuManagementAppService, IMenuAppService
{
    protected MenuManager MenuManager { get; } = menuManager;
    protected IAbpAuthorizationPolicyProvider AbpAuthorizationPolicyProvider { get; } = abpAuthorizationPolicyProvider;
    protected AbpLocalizationOptions AbpLocalizationOptions { get; } = options.Value;
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;

    [UnitOfWork]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<MenuDto> GetAsync(Guid id)
    {
        var entity = await MenuManager.GetAsync(id, new TreeIncludeOptions());
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [UnitOfWork]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        List<Menu> list;
        if (!string.IsNullOrEmpty(input.Filter))
        {
            list = await MenuManager.GetSearchListAsync(m =>
                m.Name.Contains(input.Filter) || (!string.IsNullOrEmpty(m.Router) && m.Router.Contains(input.Filter)));
        }
        else
        {
            list = await MenuManager.GetListAsync(m => m.ParentId == input.ParentId);
        }

        var dtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);

        foreach (var dto in dtos)
        {
            dto.Leaf = !await MenuManager.HasChildrenAsync(dto.Id);
        }

        return new ListResultDto<MenuDto>(dtos);
    }


    //获取用于显示的菜单列表
    [UnitOfWork]
    [AllowAnonymous]
    public virtual async Task<ListResultDto<MenuDto>> GetShowListAsync(string name)
    {
        var parent =
            await MenuManager.FindAsync(m => m.ParentId == null && m.Name.ToLower() == name.ToLowerInvariant());
        if (parent is null)
        {
            throw new EntityNotFoundException(typeof(Menu), name);
        }

        var list = await MenuManager.GetListAsync(m => m.Code.StartsWith(parent.Code + ".") && m.IsEnabled);
        var dtos = await GetChildrenAsync(parent.Id, list);

        return new ListResultDto<MenuDto>(dtos);
    }


    [UnitOfWork(true)]
    [Authorize(MenuManagementPermissions.Menus.Create)]
    public virtual async Task<MenuDto> CreateAsync(MenuCreateDto input)
    {
        var entity = new Menu(GuidGenerator.Create(), input.ParentId, input.Name, CurrentTenant.Id);
        await UpdateMenuByInputAsync(entity, input);
        await MenuManager.CreateAsync(entity);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork(true)]
    public virtual async Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input)
    {
        var entity = await MenuManager.GetAsync(id);
        //CheckIsStaticMenu(entity);
        if (!string.Equals(input.Name, entity.Name, StringComparison.OrdinalIgnoreCase))
        {
            entity.Rename(input.Name);
        }

        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        await UpdateMenuByInputAsync(entity, input);
        await MenuManager.UpdateAsync(entity);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork(true)]
    public virtual async Task MoveAsync(Guid id, Guid? parentId)
    {
        await MenuManager.MoveAsync(id, parentId);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork(true)]
    public virtual async Task CopyAsync(Guid id, Guid? parentId)
    {
        await MenuManager.CopyAsync(id, parentId);
    }

    [Authorize(MenuManagementPermissions.Menus.Delete)]
    [UnitOfWork]
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await MenuManager.GetAsync(id);
        CheckIsStaticAsync(entity);
        await MenuManager.DeleteAsync(entity);
    }

    #region 多语言

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<Dictionary<string, object>> GetMultilingualAsync(Guid id)
    {
        var entity = await MenuManager.GetAsync(id);
        return entity.GetMultilingual();
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork]
    public virtual async Task UpdateMultilingualAsync(Guid id, Dictionary<string, object> input)
    {
        var validateResult = await input.MaxLengthAsync(TreeConsts.NameMaxLength);
        if (!validateResult.Success)
        {
            throw new ValueExceedsFieldLengthBusinessException(TreeConsts.NameMaxLength,
                validateResult.Value ?? string.Empty);
        }

        var languages = input.Keys.Intersect(AbpLocalizationOptions.Languages.Select(m => m.CultureName));
        var adds = new Dictionary<string, object>(input.Where(m => languages.Contains(m.Key)));
        var entity = await MenuManager.GetAsync(id);
        entity.SetMultilingual(adds);
        await MenuManager.UpdateAsync(entity);
    }

    #endregion


    #region 权限

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id)
    {
        var entity = await MenuManager.GetAsync(id);
        var menPermissions = entity.GetPermissions();
        var result = new GetPermissionListResultDto
        {
            EntityDisplayName = "M",
            Groups = []
        };

        foreach (var group in await PermissionDefinitionManager.GetGroupsAsync())
        {
            var groupDto = CreatePermissionGroupDto(group);


            var permissions = group.GetPermissionsWithChildren()
                .Where(x => x.IsEnabled);

            //Logger.LogDebug($"permissions:{System.Text.Json.JsonSerializer.Serialize(permissions)}");
            var grantInfoDtos = permissions
                .Select(m => CreatePermissionGrantInfoDto(m, menPermissions))
                .ToList();

            groupDto.Permissions.AddRange(grantInfoDtos);

            result.Groups.Add(groupDto);
        }

        return result;
    }

    [UnitOfWork]
    [Authorize(MenuManagementPermissions.Menus.Update)]
    public virtual async Task UpdatePermissionsAsync(Guid id, MenuPermissionsUpdateDto input)
    {
        var list = input.Permissions.Where(m => !string.IsNullOrEmpty(m)).ToList();
        if (list is { Count: > 0 })
        {
            var policyNames = await AbpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
            list = list.Intersect(policyNames).ToList();
        }

        var entity = await MenuManager.GetAsync(id);
        entity.SetPermissions(list);
        await MenuManager.UpdateAsync(entity);
    }

    #endregion


    protected virtual async Task<List<MenuDto>> GetChildrenAsync(Guid? id, List<Menu> menus)
    {
        var children = menus.Where(m => m.ParentId == id);
        var chilrenDtos = new List<MenuDto>();
        foreach (var menu in children)
        {
            var permissions = menu.GetPermissions().ToArray();
            MenuDto? dto = null;
            if (permissions.Length > 0)
            {
                //有权限，说明没有子节点，直接显示
                try
                {
                    var isGranted = await AuthorizationService.IsGrantedAnyAsync(permissions);
                    if (isGranted)
                    {
                        dto = ObjectMapper.Map<Menu, MenuDto>(menu);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, "权限验证失败，菜单：{Name}，权限：{Permissions}", menu.Name, permissions);
                }
            }
            else
            {
                //没有权限，先判断是否有子节点
                if (menus.Any(m => m.ParentId == menu.Id))
                {
                    //如果有子节点，则递归
                    var childDtos = await GetChildrenAsync(menu.Id, menus);
                    //如果有符合要求的子节点，则需要显示本节点
                    if (childDtos.Any())
                    {
                        dto = ObjectMapper.Map<Menu, MenuDto>(menu);
                        dto.Children = childDtos;
                    }
                }
                else
                {
                    //没有子节点，直接显示
                    dto = ObjectMapper.Map<Menu, MenuDto>(menu);
                }
            }

            if (dto is not null)
            {
                chilrenDtos.Add(dto);
            }
        }

        return chilrenDtos;
    }


    protected virtual Task UpdateMenuByInputAsync(Menu entity, MenuCreateOrUpdateDto input)
    {
        entity.SetIcon(input.Icon);
        entity.SetOrder(input.Order);
        entity.SetRouter(input.Router);

        if (input.IsEnabled)
        {
            entity.Enable();
        }
        else
        {
            entity.Disable();
        }

        return Task.CompletedTask;
    }

    protected virtual void CheckIsStaticAsync(Menu menu)
    {
        if (menu.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Menu"], menu.Name);
        }
    }

    protected virtual PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission,
        List<string> permissions)
    {
        return new PermissionGrantInfoDto
        {
            Name = permission.Name,
            DisplayName = permission.DisplayName?.Localize(StringLocalizerFactory) ?? permission.Name,
            ParentName = permission.Parent?.Name,
            AllowedProviders = permission.Providers,
            GrantedProviders = [],
            IsGranted = permissions.Contains(permission.Name)
        };
    }

    protected virtual PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
    {
        var localizableDisplayName = group.DisplayName as LocalizableString;

        return new PermissionGroupDto
        {
            Name = group.Name,
            DisplayName = group.DisplayName?.Localize(StringLocalizerFactory) ?? group.Name,
            DisplayNameKey = localizableDisplayName?.Name,
            DisplayNameResource = localizableDisplayName?.ResourceType != null
                ? LocalizationResourceNameAttribute.GetName(localizableDisplayName.ResourceType)
                : null,
            Permissions = []
        };
    }
}