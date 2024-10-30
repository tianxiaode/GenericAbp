import {
    UserManagerSettings,
    User,
    UserManagerEvents,
    UserManager,
} from "oidc-client-ts";
import { logger } from "./utils";
import { LocalStorage } from "./LocalStorage";
import { appConfig } from "./AppConfig";
import { i18n } from "./locales";
import { http } from "./http";
import { envConfig } from "./EnvConfig";

class Account {
    $className = "Account";
    userManager: UserManager | undefined;

    init = async (oidcSettings: UserManagerSettings) => {
        this.userManager = new UserManager({
            ...oidcSettings,
            extraHeaders:{
                "__tenant": ()=>{
                    return LocalStorage.getTenant() || "";
                }
            }
        });

        this.initEvents();
    };

    get currentUser() {
        return appConfig.currentUser;
    }

    login = async (username: string, password: string) => {
        try {    
            const user = await this.userManager!.signinResourceOwnerCredentials(
                {
                    username: username,
                    password: password,
                }
            );
            if (!user) {
                throw new Error("Account.InvalidUserNameOrPassword");
            }
        } catch (e: any) {
            let message = e.error_description || e.message;
            if(message.includes("locked out")){
                message = 'Account.LockedOut'
            }else if(message.includes("not allowed")){
                message = 'Account.NotAllowed'
            }else{
                message = 'Account.InvalidUserNameOrPassword'
            }
            throw new Error(message);
            
        }
    };

    logout = async () => {
        try {
            //销毁令牌
            await this.revocationToken();
            //移除用户
            this.userManager?.removeUser();
            //移除令牌
            LocalStorage.removeRefreshToken();
            LocalStorage.removeToken();
            window.location.href = "/";
        } catch (e: any) {
            logger.error(this, ["logout"], e);
        }
    };

    loginWithExternalProvider = async (provider: string) => {
        try {
            await this.userManager!.signinRedirect({
                extraQueryParams: {
                    provider: provider,
                },
            });
        } catch (e: any) {
            throw new Error(
                "Account." + e.error_description || e.message
            );
        }
    };

    register = async (username: string, email: string, password: string) => {
        try {
            return await http.post("/api/account/register", {
                userName: username,
                emailAddress: email,
                password: password,
                appName: envConfig.appName,
            });
        } catch (e) {
            throw e;
        }
    };

    sendPasswordResetCode = async (email: string) => {
        try {
            return await http.post("/api/account/send-password-reset-code", {
                email: email,
                appName: envConfig.appName,
                returnUrl: `${window.location.origin}/reset-password`,
                returnUrlHash: "",
            });
        } catch (e) {
            throw e;
        }
    };

    verifyPasswordResetToken = async(userId: string, resetToken: string, tenant: string| null) => {
        try {
            return await http.post("/api/account/verify-password-reset-token", {
                "userId": userId,
                "resetToken": resetToken
            }, {
                headers:{ '__tenant': tenant}
            });
        }catch(e){
            throw e;
        }
    }

    resetPassword = async (
        userId: string,
        resetToken: string,
        password: string
    ) => {
        try {
            return await http.post("/api/account/reset-password", {
                userId: userId,
                resetToken: resetToken,
                password: password,
            });
        } catch (e) {
            throw e;
        }
    };

    getExternalProviders = async () => {
        try {
            return await http.get("/api/external-providers");
        } catch (e) {
            throw e;
        }
    };

    getProfile = async () => {
        try {
            return await http.get("/api/account/my-profile");
        } catch (e) {
            throw e;
        }
    };

    updateProfile = async (profile: any) => {
        try {
            return await http.put("/api/account/my-profile", profile);
        } catch (e) {
            throw e;
        }
    };

    changePassword = async (currentPassword: string, newPassword: string) => {
        try {
            return await http.post("/api/account/my-profile/change-password", {
                currentPassword,
                newPassword,
            });
        } catch (e) {
            throw e;
        }
    };

    private revocationToken = async () => { 
        const revocationEndpoint = await this.userManager!.metadataService.getRevocationEndpoint();
        if(!revocationEndpoint) return;
        const refreshToken = LocalStorage.getRefreshToken();
        const clientId = this.userManager!.settings.client_id;
        if(refreshToken){
            await http.post(revocationEndpoint, 
                `token=${refreshToken}&token_type_hint=refresh_token&client_id=${clientId}`, 
                { headers: { "Content-Type": "application/x-www-form-urlencoded" } })    

        }
        const token = LocalStorage.getToken();
        if(token){
            await http.post(revocationEndpoint, 
                `token=${token}&token_type_hint=access_token&client_id=${clientId}`, 
                { headers: { "Content-Type": "application/x-www-form-urlencoded" } })    

        }
}

    private initEvents = () => {
        const events = this.userManager!.events as UserManagerEvents;
        events.addAccessTokenExpiring(() => {
            logger.debug(this, ["initEvents"], "Access token expiring");
        });

        events.addAccessTokenExpired(() => {
            logger.debug(this, ["initEvents"], "Access token expired");
            this.userManager!.signinSilent();
            logger.debug(this, ["initEvents"], "startSilentRenew");
        });

        events.addSilentRenewError((e:any) => {
            logger.debug(this, ["initEvents"], "Silent renew error", e);
        });

        events.addUserLoaded((user) => {
            logger.debug(this, ["initEvents"], "User loaded");
            this.onUserLoaded(user);
        });

        events.addUserUnloaded(() => {
            logger.debug(this, ["initEvents"], "User unloaded");
            this.onUserUnloaded();
        });

        events.addUserSignedOut(() => {
            logger.debug(this, ["initEvents"], "User signed out");
        });

        events.addUserSessionChanged(() => {
            logger.debug(this, ["initEvents"], "User session changed");
        });
    };

    private onUserLoaded = (user: User) => {
        const oldToke = LocalStorage.getToken();
        const newToken = user.access_token;
        if (oldToke !== newToken) {
            LocalStorage.setToken(user.access_token);
            LocalStorage.setRefreshToken(user.refresh_token);
            appConfig.loadConfig();
            i18n.loadLanguage();
        }
    };

    private onUserUnloaded = () => {
        LocalStorage.removeToken();
        LocalStorage.removeRefreshToken();
        appConfig.loadConfig();
        i18n.loadLanguage();
    };
}

export const account = new Account();
