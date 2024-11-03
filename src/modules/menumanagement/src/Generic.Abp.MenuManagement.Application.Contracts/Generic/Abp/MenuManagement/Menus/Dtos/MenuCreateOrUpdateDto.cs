using Generic.Abp.Extensions.Entities.Trees;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuCreateOrUpdateDto
{
    [Required]
    [DynamicMaxLength(typeof(TreeConsts), nameof(TreeConsts.NameMaxLength))]
    [DisplayName("Menu:Name")]
    public string Name { get; set; } = default!;

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.IconMaxLength))]
    [DisplayName("Menu:Icon")]
    public string? Icon { get; set; } = default!;

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.RouterMaxLength))]
    [DisplayName("Menu:Router")]
    public string? Router { get; set; } = default!;

    [DisplayName("Menu:ISEnabled")] public bool IsEnabled { get; set; } = true;

    [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.GroupNameMaxLength))]
    [DisplayName("Menu:GroupName")]
    public string GroupName { get; set; } = default!;

    [DisplayName("Menu:Order")] public int Order { get; set; } = 1;

    [DisplayName("Menu:ParentName")] public Guid? ParentId { get; set; } = default!;
}