import { UserManager, UserManagerSettings, User, UserManagerEvents } from "oidc-client-ts";
import { logger } from "./utils";
import { LocalStorage } from "./LocalStoreage";
import { appConfig } from "./AppConfig";
import { i18n } from "./locales";

class Account {
    $className = "Account";
    userManager: UserManager | undefined;


    init = async (oidcSettings: UserManagerSettings) => {
        this.userManager = new UserManager(oidcSettings);

        this.initEvents();
    }

    get currentUser(){
        return appConfig.currentUser;
    }


    login = async (username: string, password: string) => {
        try {
            const user = this.userManager!.signinResourceOwnerCredentials({
                username: username,
                password: password,
            });
            if (!user) {
                throw new Error("InvalidUsernameOrPassword");
            }
        } catch (error) {
            throw error;
        }
    }

    logout = async () => {
        try {
            await this.userManager!.signoutSilent();
        } catch (error) {
            throw error;
        }
    }

    register = async (username: string, password: string) => {
        try {
        } catch (error) {
            throw error;
        }
    }

    resetPassword = async (email: string) => {
        try {
        } catch (error) {
            throw error;
        }
    }

    private initEvents = () => {
        const events = this.userManager!.events as UserManagerEvents;
        events.addAccessTokenExpiring(() => {
            logger.debug(this, ['initEvents'], "Access token expiring");
        });

        events.addAccessTokenExpired(() => {
            logger.debug(this, ['initEvents'], "Access token expired");
        });

        events.addSilentRenewError(() => {
            logger.debug(this, ['initEvents'], "Silent renew error");
        });

        events.addUserLoaded((user) => {
            logger.debug(this, ['initEvents'], "User loaded");
            this.onUserLoaded(user);
        });

        events.addUserUnloaded(() => {
            logger.debug(this, ['initEvents'], "User unloaded");
            this.onUserUnloaded();
        });

        events.addUserSignedOut(() => {
            logger.debug(this, ['initEvents'], "User signed out");
        });

        events.addUserSessionChanged(() => {
            logger.debug(this, ['initEvents'], "User session changed");
        });
    }

    private onUserLoaded = (user: User) => {
        const oldToke = LocalStorage.getToken();
        const newToken = user.access_token;
        if(oldToke !== newToken){
            LocalStorage.setToken(user.access_token);
            appConfig.loadConfig();
            i18n.loadLanguage();
        }
    }

    private onUserUnloaded = () => {
        LocalStorage.removeToken();
        appConfig.loadConfig();
        i18n.loadLanguage();
    }

}


export const account = new Account();