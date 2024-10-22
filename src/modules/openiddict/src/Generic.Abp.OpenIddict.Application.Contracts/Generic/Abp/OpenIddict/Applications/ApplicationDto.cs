using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Applications
{
    public class ApplicationDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ApplicationType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string ClientType { get; set; } = string.Empty;
        public string ConsentType { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string DisplayNames { get; set; } = string.Empty;

        public string JsonWebKeySet { get; set; } = string.Empty;

        public List<string> Permissions { get; set; } = [];

        public List<string> PostLogoutRedirectUris { get; set; } = [];

        public List<string> Properties { get; set; } = [];

        public List<string> RedirectUris { get; set; } = [];

        public List<string> Requirements { get; set; } = [];

        public List<string> Settings { get; set; } = [];
        public string ClientUri { get; set; } = string.Empty;

        public string LogoUri { get; set; } = string.Empty;
        public string ConcurrencyStamp { get; set; } = string.Empty;
    }
}