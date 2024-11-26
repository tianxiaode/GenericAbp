using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Applications
{
    [Serializable]
    public class ApplicationDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ApplicationType { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string ClientType { get; set; } = default!;
        public string ConsentType { get; set; } = default!;
        public string DisplayNames { get; set; } = default!;
        public HashSet<string> Permissions { get; set; } = [];

        public HashSet<Uri> PostLogoutRedirectUris { get; set; } = [];

        // public Dictionary<string, object> Properties { get; set; } = [];
        public HashSet<Uri> RedirectUris { get; set; } = [];

        // public HashSet<string> Requirements { get; set; } = [];
        public Dictionary<string, string> Settings { get; set; } = [];
        public string ClientUri { get; set; } = default!;
        public string LogoUri { get; set; } = default!;
        public string ConcurrencyStamp { get; set; } = default!;
    }
}