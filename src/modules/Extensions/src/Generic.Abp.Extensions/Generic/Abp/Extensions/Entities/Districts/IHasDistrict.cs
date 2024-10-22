using System;

namespace Generic.Abp.Extensions.Entities.Districts;

public interface IHasDistrict
{
    Guid DistrictId { get; }
}