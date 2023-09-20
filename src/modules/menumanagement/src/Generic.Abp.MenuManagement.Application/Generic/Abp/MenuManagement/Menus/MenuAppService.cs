using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Generic.Abp.BusinessException;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Generic.Abp.MenuManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using System.Collections;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuAppService : MenuManagementAppService, IMenuAppService
{
    public MenuAppService(IMenuRepository repository, MenuManager menuManager)
    {
        Repository = repository;
        MenuManager = menuManager;
    }

    protected IMenuRepository Repository { get; }
    protected MenuManager MenuManager { get; }


    [UnitOfWork]
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
        var parent = await Repository.GetAsync(input.ParentId, false);
        var entity = new Menu(GuidGenerator.Create());
        await MenuManager.CreateAsync(entity);
        await UpdateMenuByInputAsync(entity, input);
        return ObjectMapper.Map<Menu, MenuDto>(entity);
    }

    [Authorize(MenuManagementPermissions.Menus.Update)]
    [UnitOfWork(true)]
    public virtual async Task<DistrictDto> UpdateAsync(Guid id, DistrictUpdateDto input)
    {
        var entity = await Repository.GetAsync(id);
        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        entity.UpdateDisplayName(input.DisplayName);
        entity.UpdatePostcode(input.Postcode);
        await DistrictManager.UpdateAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();
        var dto = new DistrictDto(entity, !await Repository.HasChildAsync(entity.Id));
        return dto;
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
        return new ListResultDto<DistrictDto>(new List<DistrictDto>()
            { new(entity, true) });
    }

    [Authorize(InfrastructuresPermissions.Districts.Default)]
    [UnitOfWork]
    public virtual async Task<ListResultDto<DistrictTranslationDto>> GetTranslationListAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        var translations = entity.GetTranslations<District, DistrictTranslation>();
        var list = ObjectMapper.Map<List<DistrictTranslation>, List<DistrictTranslationDto>>(translations);
        return new ListResultDto<DistrictTranslationDto>(list);
    }

    [Authorize(InfrastructuresPermissions.Districts.Update)]
    [UnitOfWork]
    public virtual async Task UpdateTranslationAsync(Guid id, List<DistrictTranslationUpdateDto> input)
    {
        var list = input.Where(m => !string.IsNullOrEmpty(m.DisplayName)).ToList();
        CheckTranslationInput(list);

        var entity = await Repository.GetAsync(id);
        var translations = list.Select(m => new DistrictTranslation(m.Language, m.DisplayName));
        entity.SetTranslations<District, DistrictTranslation>(translations);
        await Repository.UpdateAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [UnitOfWork]
    protected virtual void CheckTranslationInput(List<DistrictTranslationUpdateDto> input)
    {
        var errors = input
            .Where(m => m.DisplayName.Length > DistrictConsts.DisplayNameMaxLength).Select(m =>
                L[BusinessExceptionErrorCodes.ValueExceedsFieldLength].Value
                    .Replace(BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamValue, m.DisplayName).Replace(
                        BusinessExceptionErrorCodes.ValueExceedsFieldLengthParamLength,
                        DistrictConsts.DisplayNameMaxLength.ToString())).ToList();


        if (errors.Any()) throw new UserFriendlyException(string.Join("<br>", errors));
    }

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
            var parent = await Repository.FirstOrDefaultAsync(m => m.Id == input.ParentId.Value);
            if (parent != null) throw new 
            {

            }
        }
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        //(await UserManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled)).CheckErrors();
        (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name;
        user.Surname = input.Surname;

        if (input.RoleNames != null)
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }
    }


    protected List<MenuDto> MapList(List<Menu> list)
    {
        return ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);
    }
}