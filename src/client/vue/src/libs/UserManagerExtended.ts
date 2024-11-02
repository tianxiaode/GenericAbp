import { type UserManagerSettings, INavigator,  UserManager, UserManagerSettingsStore } from "oidc-client-ts";
import { LocalStorage } from "./LocalStorage";

export class UserManagerExtended extends UserManager {

    _settings: UserManagerSettings;
    constructor(settings: UserManagerSettings, redirectNavigator?: INavigator, popupNavigator?: INavigator, iframeNavigator?: INavigator){
        super(settings, redirectNavigator, popupNavigator, iframeNavigator);
        this._settings = this.settings;
    }

    initTenantHeader(): void {
        this._settings.extraHeaders = { '__tenant': LocalStorage.getTenant() || ''  }
        console.log("initTenantHeader", LocalStorage.getTenant(), this._settings);
        console.log(this.settings)
    }

}