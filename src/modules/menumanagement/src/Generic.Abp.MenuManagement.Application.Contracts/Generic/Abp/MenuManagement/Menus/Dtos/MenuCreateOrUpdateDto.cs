using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuCreateOrUpdateDto
{
    [Required]
    [DynamicMaxLength(typeof(TreeConsts), nameof(TreeConsts.DisplayNameMaxLength))]
    [DisplayName("Menus:DisplayName")]
    public string DisplayName { get; set; }

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.IconMaxLength))]
    [DisplayName("Menus:Icon")]
    public string Icon { get; set; }

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.RouterMaxLength))]
    [DisplayName("Menus:Router")]
    public string Router { get; set; }

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.GroupNameMaxLength))]
    [DisplayName("Menus:GroupName")]
    public string GroupName { get; set; }

    [DisplayName("Menus:Order")] public int Order { get; set; }

    [DisplayName("Menus:ParentName")] public Guid? ParentId { get; set; }
}