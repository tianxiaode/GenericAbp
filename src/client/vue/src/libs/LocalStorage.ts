import { envConfig } from "./EnvConfig";
import { Cookie } from "./utils";

export class LocalStorage {
    static currentStorage: Storage = localStorage;

    static setStorageType(type: string) {
        switch (type) {
            case "cookie":
                this.currentStorage = Cookie;
                break;
            case "session":
                this.currentStorage = sessionStorage;
                break;
            default:
                this.currentStorage = localStorage;
                break;
        }
    }

    static clear() {
        try {
            this.currentStorage.clear();
        } catch (error) {
            console.error("清除存储失败:", error);
        }
    }

    static setItem(key: string, value: any) {
        try {
            this.currentStorage.setItem(key, value);
        } catch (error) {
            console.error("设置存储项失败:", error);
        }
    }

    static getItem(key: string): string | null {
        try {
            return this.currentStorage.getItem(key);
        } catch (error) {
            console.error("获取存储项失败:", error);
            return null;
        }
    }

    static removeItem(key: string) {
        try {
            this.currentStorage.removeItem(key);
        } catch (error) {
            console.error("移除存储项失败:", error);
        }
    }

    static get length(): number {
        return this.currentStorage.length;
    }

    static getNumber(key: string): number | null {
        const value = this.getItem(key);
        return value !== null ? Number(value) : null;
    }

    static getBoolean(key: string): boolean | null {
        const value = this.getItem(key);
        return value !== null ? JSON.parse(value) : null;
    }

    static get(key: string): string | null {
        return this.getItem(key);
    }

    static set(key: string, value: any) {
        this.setItem(key, value);
    }

    static getJSON(key: string): any | null {
        const value = this.getItem(key);
        return value !== null ? JSON.parse(value) : null;
    }

    static getLanguage(): string {
        return this.getItem(envConfig.languageStorageName) || navigator.language;
    }

    static setLanguage(language: string) {
        this.setItem(envConfig.languageStorageName, language);
    }

    static setRedirectPath(path: string) {
        this.setItem('redirectPath', path);
    }

    static getRedirectPath(): string | null {
        return this.getItem('redirectPath');
    }

    static setTenant(tenant: string){
        this.setItem('tenant', tenant);
    }

    static getTenant(): string | null { 
        return this.getItem('tenant');
    }

    static removeTenant() {
        this.removeItem('tenant');  
    }

    static setRememberMe(rememberMe: boolean) {
        this.setItem('rememberMe', rememberMe.toString());
    }

    static getRememberMe(): boolean {
        const value = this.getItem('rememberMe');
        return value !== null ? JSON.parse(value) : false;
    }

    static removeRememberMe() {
        this.removeItem('rememberMe');  
    }

    static setResentCount(count: number){
        this.setItem('resentCount', count.toString());
    }

    static getResentCount(): number {
        const value = this.getItem('resentCount');
        return value !== null ? Number(value) : 0;
    }

    static removeResentCount() {
        this.removeItem('resentCount');     
    }

}
