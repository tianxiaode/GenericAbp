import { EntityInterface, isGranted, logger, Repository } from "../libs";

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
    displayName: string;
    value: string;
    type?: string;
    field?: string;
}

export const ApplicationTypes: Record<string, Record<string, string>> = {
    Web: { displayName:'OpenIddict.ApplicationType:Web', value: 'web' },
    Native: { displayName:'OpenIddict.ApplicationType:Native', value: 'native' },
};

export const ApplicationClientTypes: Record<string, Record<string, string>> = {
    Confidential: { displayName:'OpenIddict.ClientTypes:Confidential', value: 'confidential' },
    Public: { displayName:'OpenIddict.ClientTypes:Public', value: 'public' },
}

export const ApplicationConsentTypes: Record<string, Record<string, string>> = {
    Explicit: { displayName:'OpenIddict.ConsentTypes:Explicit', value: 'explicit' },
    External: { displayName:'OpenIddict.ConsentTypes:External', value: 'external' },
    Implicit: { displayName:'OpenIddict.ConsentTypes:Implicit', value: 'implicit' },
    Systematic: { displayName:'OpenIddict.ConsentTypes:Systematic', value:'systematic' },
}

export const ApplicationConsentTypesList = Object.values(ApplicationConsentTypes);


export const ApplicationPermissions: Record<string, Record<string, Record<string, string>>> = {
    GrantTypes:{
        AuthorizationCode: { displayName:'OpenIddict.Permissions:GrantTypes.AuthorizationCode', value: 'gt:authorization_code' },
        ClientCredentials: { displayName:'OpenIddict.Permissions:GrantTypes.ClientCredentials', value: 'gt:client_credentials' },
        DeviceCode: { displayName:'OpenIddict.Permissions:GrantTypes.DeviceCode', value: 'gt:urn:ietf:params:oauth:grant-type:device_code' },
        Implicit: { displayName:'OpenIddict.Permissions:GrantTypes.Implicit', value: 'gt:implicit' },
        Password: { displayName:'OpenIddict.Permissions:GrantTypes.Password', value: 'gt:password' },
        RefreshToken: { displayName:'OpenIddict.Permissions:GrantTypes.RefreshToken', value: 'gt:refresh_token' },
    },
    Endpoints: {
        Authorization: { displayName:'OpenIddict.Permissions:Endpoints.Authorization', value: 'ept:authorization' },
        DeviceAuthorization: { displayName:'OpenIddict.Permissions:Endpoints.DeviceAuthorization', value: 'ept:device_authorization' },
        EndSession: { displayName:'OpenIddict.Permissions:Endpoints.EndSession', value: 'ept:end_session' },
        Introspection: { displayName:'OpenIddict.Permissions:Endpoints.Introspection', value: 'ept:introspection' },
        Revocation: { displayName:'OpenIddict.Permissions:Endpoints.Revocation', value: 'ept:revocation' },
        Token: { displayName:'OpenIddict.Permissions:Endpoints.Token', value: 'ept:token' },
    },
    Scopes: {
        Address: { displayName:'OpenIddict.Permissions:Scopes.Address', value:'scp:address' },
        Email: { displayName:'OpenIddict.Permissions:Scopes.Email', value:'scp:email' },
        Phone: { displayName:'OpenIddict.Permissions:Scopes.Phone', value:'scp:phone' },
        Profile: { displayName:'OpenIddict.Permissions:Scopes.Profile', value:'scp:profile' },
        Roles: { displayName:'OpenIddict.Permissions:Scopes.Roles', value:'scp:roles' },
    },
}

export const AllApplicationPermissionsGrantTypesValue = Object.values(ApplicationPermissions.GrantTypes).map(item => item.value);

//这里需要两个转换函数，一个是将输入的权限添加前缀，另一个是将权限值去掉前缀
export const customPermissionConvert = (value: string) => {
    return value.replace('gt:', '');    
}

export const customPermissionValueConvert = (value: string) => {
    return `gt:${value}`;
}


export const ApplicationSettings: Record<string, Record<string, string>> = {
    AccessToken: { displayName:'OpenIddict.Settings:TokenLifetimes.AccessToken', value: 'tkn_lft:act' },
    AuthorizationCode: { displayName:'OpenIddict.Settings:TokenLifetimes.AuthorizationCode', value: 'tkn_lft:auc' },
    DeviceCode: { displayName:'OpenIddict.Settings:TokenLifetimes.DeviceCode', value: 'tkn_lft:dvc' },
    IdentityToken: { displayName:'OpenIddict.Settings:TokenLifetimes.IdentityToken', value: 'tkn_lft:idt' },
    RefreshToken: { displayName:'OpenIddict.Settings:TokenLifetimes.RefreshToken', value: 'tkn_lft:reft' },
    UserCode: { displayName:'OpenIddict.Settings:TokenLifetimes.UserCode', value: 'tkn_lft:usrc' },
}

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

