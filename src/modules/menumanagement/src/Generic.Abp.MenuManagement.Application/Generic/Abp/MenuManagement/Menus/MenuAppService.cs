using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.Domain.Entities;
using Generic.Abp.Domain.Extensions;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Generic.Abp.MenuManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuAppService : MenuManagementAppService, IMenuAppService
{
    public MenuAppService(IMenuRepository repository, MenuManager menuManager,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider)
    {
        Repository = repository;
        MenuManager = menuManager;
        AbpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
    }

    protected IMenuRepository Repository { get; }
    protected MenuManager MenuManager { get; }
    private IAbpAuthorizationPolicyProvider AbpAuthorizationPolicyProvider { get; }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<MenuDto> GetRootAsync()
    {
        var root = await Repository.FirstOrDefaultAsync(m => !m.ParentId.HasValue);
        return ObjectMapper.Map<Menu, MenuDto>(root);
    }


    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        if (!string.IsNullOrWhiteSpace(input.GroupName))
        {
            var groupList = await Repository.GetListByGroupAsync(input.GroupName);
            return new ListResultDto<MenuDto>(MapList(groupList));
        }

        if (!string.IsNullOrEmpty(input.Filter))
        {
            return new ListResultDto<MenuDto>(await GetFilterListAsync(input.Filter));
        }

        if (!input.Node.HasValue) return new ListResultDto<MenuDto>();

        var list = await Repository.GetListAsync(m => m.ParentId == input.Node.Value, true);

        var dtos = MapList(list);
        foreach (var dto in dtos)
        {
            dto.Leaf = await Repository.HasChildAsync(dto.Id);
        }

        return new ListResultDto<MenuDto>(dtos);
    }

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<MenuDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [UnitOfWork(true)]
    [Authorize(MenuManagementPermissions.Menus.Create)]
    public virtual async Task<MenuDto> CreateAsync(MenuCreateDto input)
    {
        var entity = new Menu(GuidGenerator.Create());
        await UpdateMenuByInputAsync(entity, input);
        await MenuManager.CreateAsync(entity);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork(true)]
    public virtual async Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input)
    {
        var entity = await Repository.GetAsync(id);
        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        await UpdateMenuByInputAsync(entity, input);
        await MenuManager.UpdateAsync(entity);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [Authorize(MenuManagementPermissions.Menus.Delete)]
    [UnitOfWork]
    public virtual async Task<ListResultDto<MenuDto>> DeleteAsync(List<Guid> ids)
    {
        var first = ids.FirstOrDefault();
        var entity = await Repository.GetAsync(first);

        if (await Repository.HasChildAsync(first))
        {
            throw new HasChildrenCanNotDeletedBusinessException(nameof(Menu), entity.DisplayName);
        }

        await Repository.DeleteAsync(entity, true);
        return new ListResultDto<MenuDto>();
    }

    #region 多语言

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<MenuTranslationDto>> GetTranslationListAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        var translations = entity.GetTranslations<Menu, MenuTranslation>();
        var list = ObjectMapper.Map<List<MenuTranslation>, List<MenuTranslationDto>>(translations);
        return new ListResultDto<MenuTranslationDto>(list);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork]
    public virtual async Task UpdateTranslationAsync(Guid id, List<MenuTranslationUpdateDto> input)
    {
        var list = input.Where(m => !string.IsNullOrEmpty(m.DisplayName)).ToList();
        CheckTranslationInput(list);

        var entity = await Repository.GetAsync(id);
        var translations = list.Select(m => new MenuTranslation(m.Language, m.DisplayName));
        entity.SetTranslations<Menu, MenuTranslation>(translations);
        await Repository.UpdateAsync(entity);
        //await CurrentUnitOfWork.SaveChangesAsync();
    }

    [UnitOfWork]
    protected virtual void CheckTranslationInput(List<MenuTranslationUpdateDto> input)
    {
        var errors = input
            .Where(m => m.DisplayName.Length > DistrictConsts.DisplayNameMaxLength).Select(m =>
                L[BusinessExceptionErrorCodes.ValueExceedsFieldLength].Value
                    .Replace(BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamValue, m.DisplayName).Replace(
                        BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamLength,
                        DistrictConsts.DisplayNameMaxLength.ToString())).ToList();


        if (errors.Any()) throw new UserFriendlyException(string.Join("<br>", errors));
    }

    #endregion

    #region 菜单组

    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<string>> GetAllGroupNamesAsync()
    {
        var list = await Repository.GetAllGroupNamesAsync();
        return new ListResultDto<string>(list);
    }

    #endregion

    #region 权限

    [UnitOfWork]
    [DisableEntityChangeTracking]
    [Authorize(MenuManagementPermissions.Menus.Default)]
    public virtual async Task<ListResultDto<string>> GetPermissionsListAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        var permissions = entity.GetPermissions();
        return new ListResultDto<string>(permissions);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork]
    public virtual async Task UpdatePermissionsAsync(Guid id, List<string> input)
    {
        var list = input.Where(m => !string.IsNullOrEmpty(m)).ToList();
        var policyNames = await AbpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
        list = list.Intersect(policyNames).ToList();

        var entity = await Repository.GetAsync(id);
        entity.SetPermissions(list);
        await Repository.UpdateAsync(entity);
        //await CurrentUnitOfWork.SaveChangesAsync();
    }

    #endregion

    [UnitOfWork]
    protected virtual async Task<List<MenuDto>> GetFilterListAsync(string filter)
    {
        var codes = await Repository.GetCodeListAsync(filter);
        if (!codes.Any()) return new List<MenuDto>();
        var ln = TreeConsts.GetCodeLength(2);
        var level2Codes = codes.Where(m => m.Length >= ln).Select(m => m[..ln]).Distinct().ToList();
        var list = await Repository.GetListAsync(m => level2Codes.Contains(m.Code));
        var dtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);
        await GetChildrenAsync(codes, dtos, ln);
        return dtos;
    }

    [UnitOfWork]
    protected virtual async Task GetChildrenAsync(List<string> codes, List<MenuDto> dtos, int ln)
    {
        var next = ln + TreeConsts.CodeUnitLength + 1;
        var childCodes = codes.Where(m => m.Length >= next).Select(m => m[..next]).Distinct().ToList();
        if (!childCodes.Any()) return;
        var allChildren = await Repository.GetListAsync(m => childCodes.Contains(m.Code));
        foreach (var dto in dtos)
        {
            var children = allChildren.Where(m => m.Code.StartsWith(dto.Code)).ToList();
            if (!children.Any())
            {
                continue;
            }

            dto.Children = ObjectMapper.Map<List<Menu>, List<MenuDto>>(children);
            dto.Leaf = false;
            await GetChildrenAsync(codes, dto.Children.ToList(), next);
        }
    }


    protected virtual async Task UpdateMenuByInputAsync(Menu entity, MenuCreateOrUpdateDto input)
    {
        if (!entity.DisplayName.Equals(input.DisplayName, StringComparison.CurrentCultureIgnoreCase))
        {
            entity.DisplayName = input.DisplayName;
        }

        if (input.ParentId.HasValue)
        {
            var parent = await Repository.FirstOrDefaultAsync(m => m.Id == input.ParentId.Value) ??
                         throw new UnknownParentBusinessException();
            entity.ParentId = parent.Id;
        }

        entity.SetGroupName(input.GroupName);
        entity.SetIcon(input.Icon);
        entity.SetOrder(input.Order);
        entity.SetRouter(input.Router);
    }


    protected List<MenuDto> MapList(List<Menu> list)
    {
        return ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);
    }
}