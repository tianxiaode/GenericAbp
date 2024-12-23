﻿using System;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.ExternalShares;

public interface IExternalShareQueryParams : IBaseQueryParams, IHasCreationTimeQuery
{
    DateTime? ExpireTimeStart { get; set; }
    DateTime? ExpireTimeEnd { get; set; }
    public Guid? OwnerId { get; set; }
}