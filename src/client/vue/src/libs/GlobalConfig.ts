import { UserManager } from "oidc-client-ts";
import { http, BaseHttp, HttpOptions } from "./http";
import { camelCase, capitalize, logger, uncapitalize } from "./utils";
import { useConfigStore } from "../store/useConfigStore";
import { GlobalConfigType } from "./GlobalConfigType";

export interface GlobalConfigOptions {
    configUrl?: string;
    configParams?: Record<string, any>;
    httpOptions?: HttpOptions;
}

export class GlobalConfig {
    [key: string]: any;
    private configUrl: string = "/api/abp/application-configuration";
    private configParams: Record<string, any> = {
        includeLocalizationResources: false,
    };
    private config: Record<string, any> = {};
    $className: string = "GlobalConfig";
    userManager: UserManager | undefined = undefined;

    constructor() {
        this.initEnv();
        this.initUserManager();
    }

    initUserManager() {
        this.userManager = new UserManager({
            authority: this.oidcAuthority,
            client_id: this.oidcClientId,
            redirect_uri: this.redirectUri,
            post_logout_redirect_uri: this.postLogoutRedirectUri,
            response_type: this.oidcResponseType,
            scope: this.oidcScope,
            silent_redirect_uri: this.silentRedirectUri,
            automaticSilentRenew: true,
            loadUserInfo: false,
        });

        // 监听用户加载事件
        this.userManager.events.addUserLoaded((user) => {
            // 如果用户信息发生变化，可能是静默续签成功
            if (user && user.access_token) {
                BaseHttp.setToken(user.access_token);
            }
        });
    }

    initHttp(httpOptions?: HttpOptions) {
        const options = {
           ...this.getDefaultHttpOptions(),
           ...httpOptions,
        };
        BaseHttp.init(options);
    }

    async init(options: GlobalConfigOptions) {
        this.initHttp(options.httpOptions);
        if (options.configUrl) this.configUrl = options.configUrl;
        if (options.configParams) this.configParams = options.configParams;

        this.loadConfig();
    }

    async loadConfig() {
        try {
            const response = await http.get<GlobalConfigType>(this.configUrl, this.configParams);
            this.config = response;
            const configStore = useConfigStore();
            configStore.refreshReadyState(!!response, response?.currentUser.isAuthenticated || false)
        } catch (error) {
            logger.error(this, `Error loading config: `, error);
        }
    }

    getLanguages() {
        return this.config?.localization?.languages || [];
    }

    async setLanguage(language: string) {
        if (!this.i18n) {
            throw new Error(
                "globalConfig.i18n is undefined. Please call globalConfig.init() first."
            );
        }
        BaseHttp.tokenStorage.setItem(BaseHttp.languageKey, language);
        await this.loadConfig();
    }

    get currentLanguage() {
        return (
            localStorage.getItem(BaseHttp.languageKey) ||
            this.config?.localization?.currentCulture?.cultureName ||
            navigator.language
        );
    }

    get currentUser() {
        return this.config?.currentUser;
    }

    get currentTenant() {
        return this.config?.currentTenant;
    }

    get pageSizes() {
        return this.defaultPageSizes.split(",").map(Number);
    }

    get pageSize() {
        return this.pageSizes[0];
    }

    get passwordComplexitySetting() {
        const settings = this.config?.setting?.values || {};
        const result: Record<string, any> = {};

        this.assignPasswordComplexitySettings(
            settings,
            result,
            "Abp.Identity.Password.RequiredLength",
            "minLength",
            parseInt
        );
        this.assignPasswordComplexitySettings(
            settings,
            result,
            "Abp.Identity.Password.RequireDigit",
            "requireDigit",
            this.toBoolean
        );
        this.assignPasswordComplexitySettings(
            settings,
            result,
            "Abp.Identity.Password.RequireLowercase",
            "requireLowercase",
            this.toBoolean
        );
        this.assignPasswordComplexitySettings(
            settings,
            result,
            "Abp.Identity.Password.RequireUppercase",
            "requireUppercase",
            this.toBoolean
        );
        this.assignPasswordComplexitySettings(
            settings,
            result,
            "Abp.Identity.Password.RequireNonAlphanumeric",
            "requireNonAlphanumeric",
            this.toBoolean
        );

        return result;
    }

    getAllPermissions(): Record<string, boolean> {
        return this.config?.auth?.grantedPolicies || {};
        
    }


    getPermissions(
        resource: string,
        pluralizeEntity: string
    ): Record<string, boolean> {
        const permissions: Record<string, boolean> =
            this.config?.auth?.grantedPolicies || {};
        const prefix = `${capitalize(resource)}.${capitalize(
            pluralizeEntity
        )}.`;
        const result: Record<string, boolean> = {};

        Object.keys(permissions).forEach((key) => {
            if (key.startsWith(prefix)) {
                result[uncapitalize(key.replace(prefix, ""))] =
                    permissions[key];
            }
        });

        result['default'] = permissions[prefix.substring(0, prefix.length - 1)];

        return result;
    }

    normalizedLanguage(language?: string) {
        language = language || this.currentLanguage;
        if (language === "zh-Hans") language = "zh-CN";
        if (language === "zh-Hant") language = "zh-TW";
        return language;
    }

    private initEnv() {
        Object.entries(process.env).forEach(([key, value]) => {
            if (key.startsWith("VITE_")) {
                const camelKey = camelCase(key.replace("VITE_", ""));
                this[camelKey] = value;
            }
        });
    }

    private getDefaultHttpOptions(): HttpOptions {
        return {
            baseUrl: this.apiUrl,
            tokenStorage: localStorage,
            tokenStorageKey: this.tokenStorageKey,
            languageKey: this.languageKey,
            authHeaderName: this.authHeader,
            xsrfCookieName: this.xsfrCookieName,
            xsrfHeaderName: this.xsfrHeaderName,
        };
    }

    private assignPasswordComplexitySettings(
        settings: Record<string, string>,
        result: Record<string, any>,
        settingKey: string,
        resultKey: string,
        transform: (value: string) => any
    ) {
        const value = settings[settingKey];
        if (value !== undefined) {
            result[resultKey] = transform(value);
        }
    }

    private toBoolean(value: string): boolean {
        return value === "True";
    }
}

export const globalConfig = new GlobalConfig();


