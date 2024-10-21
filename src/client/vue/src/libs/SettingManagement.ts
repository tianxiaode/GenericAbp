import { http } from "./http";
import { isGranted } from "./utils";

class SettingManagement {
    static $className = "SettingManagement";
    static apiSuffix = '/api/setting-management';

    static get canManagePasswordPolicy() {
        return isGranted('SettingManagement.PasswordPolicy');
    }

    static get canManageLookupPolicy() {
        return isGranted('SettingManagement.LookupPolicy');
    }

    static get canManagerIdentitySettings() {
        return SettingManagement.canManagePasswordPolicy || SettingManagement.canManageLookupPolicy;
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

    static get canManageSettings(){
        return SettingManagement.canManagerIdentitySettings || SettingManagement.canManageEmailing || SettingManagement.canManageTimeZone;
    }

    static getPasswordPolicy() {
        try {
            return http.get(SettingManagement.getUrl('password-policy'));
        } catch (error) {
            throw error;
        }
    }

    static updatePasswordPolicy(data: any) {
        try {
            return http.post(SettingManagement.getUrl('password-policy'), data);
        } catch (error) {
            throw error;
        }
    }

    static getLookupPolicy() {
        try {
            return http.get(SettingManagement.getUrl('lookup-policy'));
        } catch (error) {
            throw error;
        }
    }

    static updateLookupPolicy(data: any) {
        try {
            return http.post(SettingManagement.getUrl('lookup-policy'), data);
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

    static testEmailingSettings(data: any) {
        try {
            return http.post(settingManagement.getUrl('emailing/send-test-email'), data);
        } catch (error) {
            throw error;
        }
    }

    static getTimeZone() {
        try {
            return http.get(SettingManagement.getUrl('timezone'));
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

    static updateTimeZone(data: any) {
        try {
            return http.post(settingManagement.getUrl('timezone'), data);
        } catch (error) {
            throw error;
        }
    }

    static getUrl(url: string) {
        return `${SettingManagement.apiSuffix}/${url}`;
    }

}

export const settingManagement = SettingManagement;