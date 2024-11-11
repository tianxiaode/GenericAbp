import { http } from "./http";
import {  logger } from "./utils";
import { AppConfigType } from "./AppConfigType";

export interface AppConfigOptions {
    configUrl?: string;
    configParams?: Record<string, any>;
}

export class AppConfig {
    $className: string = "AppConfig";
    private configUrl: string = "/api/abp/application-configuration";
    private configParams: Record<string, any> = {
        includeLocalizationResources: false,
    };
    private config: Record<string, any> = {};
    private _currentUser : Record<string, any> = {};

    constructor(configUrl?: string, configParams?: Record<string, any>) {
        if (configUrl) this.configUrl = configUrl;
        if (configParams) this.configParams = configParams;
    }

    async loadConfig() {
        try {
            const response = await http.get<AppConfigType>(this.configUrl, this.configParams);
            this.config = response;
            this.currentUser = response.currentUser;
            logger.debug(this, '[loadConfig]', `Config loaded: `, this.config);
        } catch (error) {
            logger.error(this, '[loadConfig]', `Error loading config: `, error);
        }
    }

    get languages() {
        return this.config?.localization?.languages || [];
    }

    get currentCulture() {
        return this.config?.localization?.currentCulture;
    }

    get currentUser() {
        return this._currentUser;
    }

    set currentUser(user: Record<string, any>) {
        this._currentUser = user;
    }

    get currentTenant() {
        return this.config?.currentTenant;
    }

    get passwordComplexitySetting() {
        const settings = this.config?.setting?.values || {};

        return {
            minLength: parseInt(settings["Abp.Identity.Password.RequiredLength"]),
            requireDigit: settings["Abp.Identity.Password.RequireDigit"] === "True",
            requireLowercase: settings["Abp.Identity.Password.RequireLowercase"] === "True",
            requireUppercase: settings["Abp.Identity.Password.RequireUppercase"] === "True",
            requireNonAlphanumeric: settings["Abp.Identity.Password.RequireNonAlphanumeric"] === "True",
        }

    }

    get permissions(): Record<string, boolean> {
        return this.config?.auth?.grantedPolicies || {};
        
    }

    get isEmailUpdateEnabled() {
        return this.config?.setting?.values?.["Abp.Identity.User.IsEmailUpdateEnabled"] === "True";
    }

    get isUserNameUpdateEnabled(){
        return this.config?.setting?.values?.["Abp.Identity.User.IsUserNameUpdateEnabled"] === "True";
    }

    get enableMultiTenancy(){
        return this.config?.multiTenancy?.isEnabled;
    }


}

export const appConfig = new AppConfig();


