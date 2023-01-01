

using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Scopes
{
    public class ScopeDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public List<string> Properties { get; set; }
        public List<string> Resources { get; set; }
        public string ConcurrencyStamp { get; set; }

    }
}
