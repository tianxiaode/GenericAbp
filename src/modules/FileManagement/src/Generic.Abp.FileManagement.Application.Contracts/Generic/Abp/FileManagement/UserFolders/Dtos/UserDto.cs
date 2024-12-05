using System;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = default!;
}