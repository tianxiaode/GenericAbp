import {
    UserManagerSettings,
    User,
    UserManagerEvents,
    UserManager,
    WebStorageStateStore,
} from "oidc-client-ts";
import { logger } from "./utils";
import { LocalStorage } from "./LocalStorage";
import { appConfig } from "./AppConfig";
import { http } from "./http";
import { envConfig } from "./EnvConfig";

class Account {
    $className = "Account";
    userManager: UserManager | undefined;
    user: User | null = null;
    userManagerSettings: any;
    reloadConfig = () => {};
    init = async (oidcSettings: UserManagerSettings, reloadConfig: () => {}) => {
        this.userManager = new UserManager({
            ...oidcSettings,
            extraHeaders:{
                "__tenant": ()=>{
                    return LocalStorage.getTenant() || "";
                }
            }
        });
        //从localStorage中获取用户
        this.reloadConfig = reloadConfig;
        this.userManagerSettings = this.userManager.settings;
        //根据rememberMe状态，设置userStore
        this.resetUserStore(LocalStorage.getRememberMe() || false);
        this.user = await this.userManager.getUser();
        this.initEvents();
        logger.debug(this, "[init]", "User:", this.user);
    };

    get currentUser() {
        return appConfig.currentUser;
    }

    login = async (username: string, password: string, rememberMe: boolean) => {
        try {    
            rememberMe = rememberMe === true ? true : false;
            //设置rememberMe状态，以便重新进入应用后从何处获取用户信息
            LocalStorage.setRememberMe(rememberMe);
            //根据rememberMe状态，设置userStore
            this.resetUserStore(rememberMe);
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
            logger.error(this, ["login"], e);
            throw new Error(message);
            
        }
    };

    logout = async () => {
        try {
            //主动退出，清理rememberMe状态
            LocalStorage.removeRememberMe();
            //销毁令牌
            await this.revocationToken();
            //移除用户
            this.userManager?.removeUser();
            //重新加载配置
            this.reloadConfig();
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

    getAccessToken = () => {
        return this.user?.access_token;
    }

    getTokenType = () => {  
        return this.user?.token_type;
    }

    resetUserStore = (rememberMe: boolean) =>{
        this.userManagerSettings.userStore = new WebStorageStateStore({
            store: rememberMe ? localStorage : sessionStorage
        });    
    }

    signinRedirectCallback = async () => {
        return this.userManager?.signinRedirectCallback();
    }

    protected revocationToken = async () => { 
        logger.debug(this, ["revocationToken"], "Starting token revocation");
        const revocationEndpoint = await this.userManager!.metadataService.getRevocationEndpoint();
        if(!revocationEndpoint) return;
        const refreshToken = this.user?.refresh_token;
        const clientId = this.userManager!.settings.client_id;
        if(refreshToken){
            await http.post(revocationEndpoint, 
                `token=${refreshToken}&token_type_hint=refresh_token&client_id=${clientId}`, 
                { headers: { "Content-Type": "application/x-www-form-urlencoded" } })    

        }
        const token = this.user?.access_token;
        if(token){
            await http.post(revocationEndpoint, 
                `token=${token}&token_type_hint=access_token&client_id=${clientId}`, 
                { headers: { "Content-Type": "application/x-www-form-urlencoded" } })    

        }
    }

    protected initEvents = () => {
        const events = this.userManager!.events as UserManagerEvents;
        events.addAccessTokenExpiring(() => {
            this.onAccessTokenExpiring();
        });

        events.addAccessTokenExpired(() => {
            this.onAccessTokenExpired();
        });

        events.addSilentRenewError((e:any) => {
            logger.debug(this, "[initEvents]", "Silent renew error", e);
        });

        events.addUserLoaded((user) => {
            logger.debug(this, "[initEvents]", "User loaded");
            this.onUserLoaded(user);
        });

        events.addUserUnloaded(() => {
            logger.debug(this, "[initEvents]", "User unloaded");
            this.onUserUnloaded();
        });

        events.addUserSignedOut(() => {
            logger.debug(this, "[initEvents]", "User signed out");
        });

        events.addUserSessionChanged(() => {
            logger.debug(this, "[initEvents]", "User session changed");
        });
    };

    protected onUserLoaded = (user: User) => {
        //用户加载后，刷新用户，重新加载配置文件和语言文件
        logger.debug(this, "[onUserLoaded]", "User loaded", user);
        this.user = user;
        this.reloadConfig();
    };


    protected onUserUnloaded = () => {
        logger.debug(this, "[onUserUnloaded]", "User unloaded");
    };

    protected onAccessTokenExpiring = () =>{
        logger.debug(this, "[onAccessTokenExpiring]", "Access token expiring");
    }

    protected onAccessTokenExpired = () => {
        logger.debug(this, "[onAccessTokenExpired]", "Access token expired");
    }
}

export const account = new Account();
