﻿using AutoMapper;
using Generic.Abp.OpenIddict.Applications;
using Generic.Abp.OpenIddict.Scopes;
using System.Collections.Generic;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace Generic.Abp.OpenIddict
{
    public class OpenIddictApplicationAutoMapperProfile : Profile
    {
        public OpenIddictApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<OpenIddictScope, ScopeDto>()
                .ForMember(m => m.Properties, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.Properties, new System.Text.Json.JsonSerializerOptions())))
                .ForMember(m => m.Resources, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.Resources, new System.Text.Json.JsonSerializerOptions())));

            CreateMap<OpenIddictApplication, ApplicationDto>();
            //.ForMember(m => m.Properties, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.Properties, new System.Text.Json.JsonSerializerOptions())))
            //.ForMember(m => m.PostLogoutRedirectUris, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.PostLogoutRedirectUris, new System.Text.Json.JsonSerializerOptions())))
            //.ForMember(m => m.RedirectUris, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.RedirectUris, new System.Text.Json.JsonSerializerOptions())))
            //.ForMember(m => m.Requirements, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.Requirements, new System.Text.Json.JsonSerializerOptions())))
            //.ForMember(m => m.Permissions, opts => opts.MapFrom(m => System.Text.Json.JsonSerializer.Deserialize<List<string>>(m.Permissions, new System.Text.Json.JsonSerializerOptions())));
        }
    }
}