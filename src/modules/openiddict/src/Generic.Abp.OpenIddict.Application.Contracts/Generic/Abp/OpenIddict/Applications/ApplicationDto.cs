using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Applications
{
    public class ApplicationDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ConsentType { get; set; }

        public string DisplayName { get; set; }

        //public List<string> Permissions { get; set; }

        //public List<string> PostLogoutRedirectUris { get; set; }

        //public List<string> Properties { get; set; }

        //public List<string> RedirectUris { get; set; }

        //public List<string> Requirements { get; set; }

        public string Type { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
