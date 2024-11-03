// src/store/config.ts
import { defineStore } from 'pinia';
import { appConfig, i18n, LocalStorage } from '~/libs';

interface ConfigState {
    locale: string;
    isLocaleReady: boolean;
    isConfigReady: boolean;
    isAuthenticated: boolean;
    isFist: boolean;
}

export const useConfigStore = defineStore('config', {
    state: (): ConfigState => ({
        locale: LocalStorage.getLanguage(),
        isLocaleReady: false,
        isConfigReady: false,
        isAuthenticated: false,
        isFist: true,
    }),
    actions: {
        async setLocale(locale: string) {
            if (!this.isFist && this.locale === locale) return;

            this.isFist = false;
            this.locale = locale;
            LocalStorage.setLanguage(locale);

            await this.updateI18n();
            await this.loadAppConfig();
        },

        async updateI18n() {
            await i18n.loadLanguage();
            this.isLocaleReady = true;
        },

        async loadAppConfig() {
            await appConfig.loadConfig();
            this.isConfigReady = true;
            this.isAuthenticated = appConfig.currentUser?.isAuthenticated || false;
        },
    },
});