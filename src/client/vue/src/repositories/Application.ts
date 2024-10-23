import exp from "constants";
import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface ApplicationType extends EntityInterface {
    applicationType: string;
    clientId: String;
    clientSecret: String;
    clientType: String;
    consentType: String;
    displayName: String;
    displayNames: String;
    jsonWebKeySet: String;
    permissions: String[];
    postLogoutRedirectUris: String[];
    properties: String[];
    redirectUris: String[];
    requirements: String[];
    settings: String[];
    clientUri: String;
    logoUri: String;
}

export interface SelectionOption {
    display: string;
    value: string;
    type?: string;
}

export const ApplicationTypes: SelectionOption[] = [
    { display: "OpenIddict.ApplicationType:Native", value: "native" },
    { display: "OpenIddict.ApplicationType:Web", value: "web" },
];

export const ApplicationClientTypes: SelectionOption[] = [
{ display: "OpenIddict.ClientTypes:Confidential", value: "confidential" },
{ display: "OpenIddict.ClientTypes:Public", value: "public" },
];

export const ApplicationConsentTypes: SelectionOption[] = [
    { display: "OpenIddict.ConsentTypes:Explicit", value: "explicit" },
    { display: "OpenIddict.ConsentTypes:External", value: "external" },
    { display: "OpenIddict.ConsentTypes:Implicit", value: "implicit" },
    { display: "OpenIddict.ConsentTypes:Systematic", value: "systematic" },
];

export const ApplicationPermissions: SelectionOption[] = [
    { display: "OpenIddict.Permissions:Endpoints.Authorization", type:'endpoint', value: "ept:authorization" },
    { display: "OpenIddict.Permissions:Endpoints.DeviceAuthorization", type:'endpoint', value: "ept:device_authorization" },
    { display: "OpenIddict.Permissions:Endpoints.EndSession", type:'endpoint', value: "ept:end_session" },
    { display: "OpenIddict.Permissions:Endpoints.Introspection", type:'endpoint', value: "ept:introspection" },
    { display: "OpenIddict.Permissions:Endpoints.Revocation", type:'endpoint', value: "ept:revocation" },
    { display: "OpenIddict.Permissions:Endpoints.Token", type:'endpoint', value: "ept:token" },
    { display: "OpenIddict.Permissions:GrantTypes.AuthorizationCode", type:'grant_type', value: "gt:authorization_code" },
    { display: "OpenIddict.Permissions:GrantTypes.ClientCredentials", type:'grant_type', value: "gt:client_credentials" },
    { display: "OpenIddict.Permissions:GrantTypes.DeviceCode", type:'grant_type', value: "gt:urn:ietf:params:oauth:grant-type:device_code" },
    { display: "OpenIddict.Permissions:GrantTypes.Implicit", type:'grant_type', value: "gt:implicit" },
    { display: "OpenIddict.Permissions:GrantTypes.Password", type:'grant_type', value: "gt:password" },
    { display: "OpenIddict.Permissions:GrantTypes.RefreshToken", type:'grant_type', value: "gt:refresh_token" },
];

export const ApplicationDefaultScopes: SelectionOption[] = [
    { display: "OpenIddict.Permissions:Scopes.Address", value: "scp:address" },
    { display: "OpenIddict.Permissions:Scopes.Email", value: "scp:email" },
    { display: "OpenIddict.Permissions:Scopes.Phone", value: "scp:phone" },
    { display: "OpenIddict.Permissions:Scopes.Profile", value: "scp:profile" },
    { display: "OpenIddict.Permissions:Scopes.Roles", value: "scp:roles" },
]

export const ApplicationSettings: SelectionOption[] = [
    { display: "OpenIddict.Settings:TokenLifetimes.AccessToken", value: 'tkn_lft:access_token' },
    { display: "OpenIddict.Settings:TokenLifetimes.AuthorizationCode", value: 'tkn_lft:auc' }, 
    { display: "OpenIddict.Settings:TokenLifetimes.DeviceCode", value: 'tkn_lft:dvc' },
    { display: "OpenIddict.Settings:TokenLifetimes.IdentityToken", value: 'tkn_lft:idt' },
    { display: "OpenIddict.Settings:TokenLifetimes.RefreshToken", value: 'tkn_lft:reft' },
    { display: "OpenIddict.Settings:TokenLifetimes.UserCode", value: 'tkn_lft:usrc' },
]

export class ApplicationRepository extends Repository<ApplicationType> {
    $className = 'ApplicationRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'RoleRepository initialized')
        this.entity = 'application';
        this.resourceName = "OpenIddict";
        this.entityGroup = "";  
        this.messageField = "name";
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}.ManagePermissions`);
    }

}

