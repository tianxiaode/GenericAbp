import { http } from "./http";
import { capitalize, logger, uncapitalize } from "./utils";
import { useConfigStore } from "~/store";
import { AppConfigType } from "./AppConfigType";
import { i18n } from "./locales";
import { ElMessage } from "element-plus";

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

    constructor(configUrl?: string, configParams?: Record<string, any>) {
        if (configUrl) this.configUrl = configUrl;
        if (configParams) this.configParams = configParams;
    }

    async loadConfig() {
        try {
            
            const configStore = useConfigStore();
            configStore.refreshState(false, false)
            const response = await http.get<AppConfigType>(this.configUrl, this.configParams);
            this.config = response;
            // if(this.currentUser.isAuthenticated){
            //     this.checkNeedSetPassword();
            // }
            configStore.refreshState(!!response, response?.currentUser.isAuthenticated || false)
        } catch (error) {
            logger.error(this, `Error loading config: `, error);
        }
    }

    getLanguages() {
        return this.config?.localization?.languages || [];
    }

    get currentCulture() {
        return this.config?.localization?.currentCulture;
    }

    get currentUser() {
        return this.config?.currentUser;
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



    // getPermissions(
    //     resource: string,
    //     pluralizeEntity: string
    // ): Record<string, boolean> {
    //     const permissions: Record<string, boolean> =
    //         this.config?.auth?.grantedPolicies || {};
    //     const prefix = `${capitalize(resource)}.${capitalize(
    //         pluralizeEntity
    //     )}.`;
    //     const result: Record<string, boolean> = {};

    //     Object.keys(permissions).forEach((key) => {
    //         if (key.startsWith(prefix)) {
    //             result[uncapitalize(key.replace(prefix, ""))] =
    //                 permissions[key];
    //         }
    //     });

    //     result['default'] = permissions[prefix.substring(0, prefix.length - 1)];

    //     return result;
    // }

    private async checkNeedSetPassword(): Promise<void> {
        const result = await http.get("/need-set-password") as any;
        if(result.need){
            ElMessage.error(i18n.get('Message.ResetPasswordTip'));
        }
    }
}

export const appConfig = new AppConfig();


