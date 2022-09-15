using System;

namespace Generic.Abp.Domain.Entities;

public interface IHasDistrict
{
    Guid DistrictId { get; }
}