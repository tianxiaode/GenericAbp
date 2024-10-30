class EnvConfig {
    oidcAuthority: string;
    oidcClientId: string;
    oidcScope: string;
    oidcResponseType: string;
    apiUrl: string | undefined;
    xsrfCookieName: string | undefined;
    xsrfHeaderName: string | undefined;
    baseUrl: string | undefined;
    publishPath: string | undefined;
    languageStorageName: string;
    authHeaderName: string | undefined;
    defaultDateFormat: string | undefined;
    defaultDateTimeFormat: string | undefined;
    defaultTimeFormat: string | undefined;
    defaultDecimalPrecision: number | undefined;
    defaultArrayFormat: string | undefined;
    defaultPageSizes: number[];
    defaultPageSize: number;
    appName: string;

    constructor() {
        const env = process.env;
        this.oidcAuthority = env.VITE_OIDC_AUTHORITY || '';
        this.oidcClientId = env.VITE_OIDC_CLIENT_ID || '';
        this.oidcScope = env.VITE_OIDC_SCOPE || 'openid profile email offline_access';
        this.oidcResponseType = env.VITE_OIDC_RESPONSE_TYPE || 'code';
        this.apiUrl = env.VITE_API_URL;
        this.xsrfCookieName = env.VITE_XSRF_COOKIE_NAME;
        this.xsrfHeaderName = env.VITE_XSRF_HEADER_NAME;
        this.baseUrl = env.VITE_BASE_URL;
        this.publishPath = env.VITE_PUBLISH_PATH;
        this.languageStorageName = env.VITE_LANGUAGE_STORAGE_NAME || 'language';
        this.authHeaderName = env.VITE_AUTH_HEADER_NAME  || 'Authorization';
        this.defaultDateFormat = env.VITE_DEFAULT_DATE_FORMAT;
        this.defaultDateTimeFormat = env.VITE_DEFAULT_DATETIME_FORMAT;
        this.defaultTimeFormat = env.VITE_DEFAULT_TIME_FORMAT;
        this.defaultDecimalPrecision = Number(env.VITE_DEFAULT_DECIMAL_PRECISION);
        this.defaultArrayFormat = env.VITE_DEFAULT_ARRAY_FORMAT;
        this.defaultPageSizes = env.VITE_DEFAULT_PAGE_SIZES?.split(',').map(Number) || [10, 20, 30, 50, 100];
        this.defaultPageSize = this.defaultPageSizes[0] || 10;
        this.appName = env.VITE_APP_NAME || 'QuickTemplate';
    }

    get defaultHttpConfig(): any {
        return {
            baseUrl: this.apiUrl,
            tokenStorage: localStorage,
            languageKey: this.languageStorageName,
            authHeaderName: this.authHeaderName,
            xsrfCookieName: this.xsrfCookieName,
            xsrfHeaderName: this.xsrfHeaderName,

        };
    }
}

export const envConfig = new EnvConfig();