using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuUpdateDto : MenuCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;
}