import { http } from "./http";
import { isGranted } from "./utils";

class SettingManagement {
    static $className = "SettingManagement";
    static apiSuffix = '/api/setting-management';

    static get canManageIdentitySettings() {
        return isGranted('SettingManagement.IdentityManagement');
    }

    static get canManageEmailingSettings(){
        return isGranted('SettingManagement.Emailing');
    }

    static get canManageEmailingTest(){
        return isGranted('SettingManagement.Emailing.Test');
    }

    static get canManageEmailing(){
        return SettingManagement.canManageEmailingSettings || SettingManagement.canManageEmailingTest;
    }

    static get canManageTimeZone(){
        return isGranted('SettingManagement.TimeZone');
    }

    static get canManageExternalAuthentication(): boolean {
        return isGranted('SettingManagement.ExternalAuthenticationManagement');
    }

    static get canManageSettings(){
        return SettingManagement.canManageIdentitySettings || 
            SettingManagement.canManageEmailing || 
            SettingManagement.canManageTimeZone || 
            SettingManagement.canManageExternalAuthentication;
    }


    static getIdentitySettings() {
        try {
            return http.get(SettingManagement.getUrl('identity'));
        } catch (error) {
            throw error;
        }
    }

    static updateIdentitySettings(data: any) {
        try {
            return http.put(SettingManagement.getUrl('identity'), data);
        } catch (error) {
            throw error;
        }
    }


    static getEmailingSettings() {
        try {
            return http.get(SettingManagement.getUrl('emailing'));
        } catch (error) {
            throw error;
        }
    }

    static updateEmailingSettings(data: any) {
        try {
            return http.post(settingManagement.getUrl('emailing'), data);
        } catch (error) {
            throw error;
        }
    }

    static sendTestEmail(data: any) {
        try {
            return http.post(settingManagement.getUrl('emailing/send-test-email'), data, {
                timeout: 100000,
            });
        } catch (error) {
            throw error;
        }
    }

    static getTimeZone() {
        try {
            return http.get(SettingManagement.getUrl('timezone'),{},{
                responseType:'text'
            });
        } catch (error) {
            throw error;
        }
    }

    static getTimeZones() {
        try {
            return http.get(SettingManagement.getUrl('timezone/timezones'));
        } catch (error) {
            throw error;
        }
    }

    static updateTimeZone(timezone: string) {
        try {
            return http.post(settingManagement.getUrl('timezone') +'?timezone='+timezone, {});
        } catch (error) {
            throw error;
        }
    }

    static getExternalAuthenticationSettings() {
        try {
            return http.get(SettingManagement.getUrl('external-authentication'));
        } catch (error) {
            throw error;
        }
    }

    static updateExternalAuthenticationSettings(data: any) {
        try {
            return http.put(SettingManagement.getUrl('external-authentication'), data);
        } catch (error) {
            throw error;
        }
    }   

    static getUrl(url: string) {
        return `${SettingManagement.apiSuffix}/${url}`;
    }

}

export const settingManagement = SettingManagement;