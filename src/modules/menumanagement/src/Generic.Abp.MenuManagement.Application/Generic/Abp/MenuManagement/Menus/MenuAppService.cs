using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.MultiLingual;
using Generic.Abp.Extensions.Entities.Permissions;
using Generic.Abp.Extensions.Entities.Trees;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Validates;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Generic.Abp.MenuManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuAppService : MenuManagementAppService, IMenuAppService
{
    public MenuAppService(IMenuRepository repository, MenuManager menuManager,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider, IOptions<AbpLocalizationOptions> options)
    {
        Repository = repository;
        MenuManager = menuManager;
        AbpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
        AbpLocalizationOptions = options.Value;
    }

    protected IMenuRepository Repository { get; }
    protected MenuManager MenuManager { get; }
    protected IAbpAuthorizationPolicyProvider AbpAuthorizationPolicyProvider { get; }
    protected AbpLocalizationOptions AbpLocalizationOptions { get; }

    [UnitOfWork]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<MenuDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [UnitOfWork]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        List<Menu> list;
        if (!string.IsNullOrEmpty(input.Filter))
        {
            list = await GetSearchMenuList(input);
        }
        else
        {
            list = await Repository.GetListAsync(m => m.ParentId == input.ParentId);
        }

        var dtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);

        foreach (var dto in dtos)
        {
            dto.Leaf = !await Repository.HasChildAsync(dto.Id);
        }

        return new ListResultDto<MenuDto>(dtos);
    }

    //获取用于显示的菜单列表
    [UnitOfWork]
    [AllowAnonymous]
    public virtual async Task<ListResultDto<MenuDto>> GetShowListAsync(string name)
    {
        var parent =
            await Repository.FirstOrDefaultAsync(m =>
                m.Name.ToLower() == name.ToLowerInvariant() && m.ParentId == null);
        if (parent is null)
        {
            throw new EntityNotFoundException(typeof(Menu), name);
        }

        var list = await Repository.GetListAsync(m => m.Code.StartsWith(parent.Code + ".") && m.IsEnabled);
        var dtos = await GetChildrenAsync(parent.Id, list);

        return new ListResultDto<MenuDto>(dtos);
    }


    [UnitOfWork(true)]
    //[Authorize(MenuManagementPermissions.Menus.Create)]
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
        var entity = await Repository.GetAsync(id);
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

    [Authorize(MenuManagementPermissions.Menus.Delete)]
    [UnitOfWork]
    public virtual async Task<ListResultDto<MenuDto>> DeleteAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        CheckIsStaticMenu(entity);
        if (await Repository.HasChildAsync(id))
        {
            throw new HasChildrenCanNotDeletedBusinessException(nameof(Menu), entity.Name);
        }

        await Repository.DeleteAsync(entity, true);
        return new ListResultDto<MenuDto>();
    }

    #region 多语言

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<Dictionary<string, object>> GetMultiLingualAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return entity.GetMultiLingual();
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork]
    public virtual async Task UpdateMultiLingualAsync(Guid id, Dictionary<string, object> input)
    {
        var validateResult = await input.MaxLengthAsync(TreeConsts.NameMaxLength);
        if (!validateResult.Success)
        {
            throw new ValueExceedsFieldLengthBusinessException(TreeConsts.NameMaxLength,
                validateResult.Value ?? string.Empty);
        }

        var languages = input.Keys.Intersect(AbpLocalizationOptions.Languages.Select(m => m.CultureName));
        var adds = new Dictionary<string, object>(input.Where(m => languages.Contains(m.Key)));
        var entity = await Repository.GetAsync(id);
        entity.SetMultiLingual(adds);
        await Repository.UpdateAsync(entity, true);
    }

    #endregion


    #region 权限

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<List<string>> GetPermissionsListAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return entity.GetPermissions().ToList();
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork]
    public virtual async Task UpdatePermissionsAsync(Guid id, List<string> input)
    {
        var list = input.Where(m => !string.IsNullOrEmpty(m)).ToList();
        if (list is { Count: > 0 })
        {
            var policyNames = await AbpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
            list = list.Intersect(policyNames).ToList();
        }

        var entity = await Repository.GetAsync(id);
        entity.SetPermissions(list);
        await Repository.UpdateAsync(entity);
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

    protected virtual async Task<List<Menu>> GetSearchMenuList(MenuGetListInput input)
    {
        //先找出所有符合条件的code
        var codes = await Repository.GetAllCodesByFilterAsync(input.Filter!, input.GroupName);
        if (!codes.Any())
        {
            return [];
        }


        var allParents = await GetAllParents(codes);

        return await Repository.GetListAsync(m => allParents.Contains(m.Code));
    }


    protected virtual Task<HashSet<string>> GetAllParents(List<string> codes)
    {
        var parents = new HashSet<string>();
        foreach (var parts in codes.Select(code => code.Split('.')))
        {
            for (var i = 1; i <= parts.Length; i++)
            {
                var parent = string.Join(".", parts.Take(i));
                parents.Add(parent);
            }
        }

        return Task.FromResult(parents);
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

    protected virtual void CheckIsStaticMenu(Menu menu)
    {
        if (menu.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Menu"], menu.Name);
        }
    }
}