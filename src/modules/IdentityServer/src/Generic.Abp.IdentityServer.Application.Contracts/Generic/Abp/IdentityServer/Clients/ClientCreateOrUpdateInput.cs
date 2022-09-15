using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientCreateOrUpdateInput
{
    [Required]
    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientIdMaxLength))]
    [DisplayName("Client:ClientId")]
    public string ClientId { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientNameMaxLength))]
    [DisplayName("Client:ClientName")]
    public string ClientName { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.DescriptionMaxLength))]
    [DisplayName("Client:Description")]
    public string Description { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientUriMaxLength))]
    [DisplayName("Client:ClientUri")]
    public string ClientUri { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.LogoUriMaxLength))]
    [DisplayName("Client:LogoUri")]
    public string LogoUri { get; set; }

    [DisplayName("Client:Enabled")]
    public bool Enabled { get; set; } = true;

    [Required]
    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ProtocolTypeMaxLength))]
    [DisplayName("Client:ProtocolType")]
    public string ProtocolType { get; set; }

    [DisplayName("Client:RequireClientSecret")]
    public bool RequireClientSecret { get; set; }

    [DisplayName("Client:RequireConsent")]
    public bool RequireConsent { get; set; }

    [DisplayName("Client:AllowRememberConsent")]
    public bool AllowRememberConsent { get; set; }

    [DisplayName("Client:AlwaysIncludeUserClaimsInIdToken")]
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    [DisplayName("Client:RequirePkce")]
    public bool RequirePkce { get; set; }

    [DisplayName("Client:AllowPlainTextPkce")]
    public bool AllowPlainTextPkce { get; set; }

    [DisplayName("Client:AllowAccessTokensViaBrowser")]
    public bool AllowAccessTokensViaBrowser { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.FrontChannelLogoutUriMaxLength))]
    [DisplayName("Client:FrontChannelLogoutUri")]
    public string FrontChannelLogoutUri { get; set; }

    [DisplayName("Client:FrontChannelLogoutSessionRequired")]
    public bool FrontChannelLogoutSessionRequired { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.BackChannelLogoutUriMaxLength))]
    [DisplayName("Client:BackChannelLogoutUri")]
    public string BackChannelLogoutUri { get; set; }

    [DisplayName("Client:BackChannelLogoutSessionRequired")]
    public bool BackChannelLogoutSessionRequired { get; set; }

    [DisplayName("Client:AllowOfflineAccess")]
    public bool AllowOfflineAccess { get; set; }

    [DisplayName("Client:IdentityTokenLifetime")]
    public int IdentityTokenLifetime { get; set; }

    [DisplayName("Client:AccessTokenLifetime")]
    public int AccessTokenLifetime { get; set; }

    [DisplayName("Client:AuthorizationCodeLifetime")]
    public int AuthorizationCodeLifetime { get; set; }

    [DisplayName("Client:ConsentLifetime")]
    public int? ConsentLifetime { get; set; }

    [DisplayName("Client:AbsoluteRefreshTokenLifetime")]
    public int AbsoluteRefreshTokenLifetime { get; set; }

    [DisplayName("Client:SlidingRefreshTokenLifetime")]
    public int SlidingRefreshTokenLifetime { get; set; }

    [DisplayName("Client:RefreshTokenUsage")]
    public int RefreshTokenUsage { get; set; }

    [DisplayName("Client:UpdateAccessTokenClaimsOnRefresh")]
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    [DisplayName("Client:RefreshTokenExpiration")]
    public int RefreshTokenExpiration { get; set; }

    [DisplayName("Client:AccessTokenType")]
    public int AccessTokenType { get; set; }

    [DisplayName("Client:EnableLocalLogin")]
    public bool EnableLocalLogin { get; set; }

    [DisplayName("Client:IncludeJwtId")]
    public bool IncludeJwtId { get; set; }

    [DisplayName("Client:AlwaysSendClientClaims")]
    public bool AlwaysSendClientClaims { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientClaimsPrefixMaxLength))]
    [DisplayName("Client:ClientClaimsPrefix")]
    public string ClientClaimsPrefix { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.PairWiseSubjectSaltMaxLength))]
    [DisplayName("Client:PairWiseSubjectSalt")]
    public string PairWiseSubjectSalt { get; set; }

    [DisplayName("Client:UserSsoLifetime")]
    public int? UserSsoLifetime { get; set; }

    [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.UserCodeTypeMaxLength))]
    [DisplayName("Client:UserCodeType")]
    public string UserCodeType { get; set; }

    [DisplayName("Client:DeviceCodeLifetime")]
    public int DeviceCodeLifetime { get; set; } = 300;

}