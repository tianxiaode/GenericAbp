using System;
using System.Collections.Generic;

namespace Generic.Abp.Extensions.Entities.Dtos;

public class DeleteManyDto
{
    public List<Guid> Ids { get; set; } = default!;
}