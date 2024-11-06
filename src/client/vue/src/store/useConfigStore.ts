// src/store/config.ts
import { defineStore } from 'pinia';
import { appConfig, i18n, LocalStorage, logger } from '~/libs';

interface ConfigState {
    locale: string;
    isLocaleReady: boolean;
    isConfigReady: boolean;
    isFirstLoad: boolean;
}

export const useConfigStore = defineStore('config', {
    state: (): ConfigState => ({
        locale: LocalStorage.getLanguage() || 'en', // 确保有默认值
        isLocaleReady: false,
        isConfigReady: false,
        isFirstLoad: true,
    }),
    actions: {
        async setLocale(locale: string) {
            if (!this.isFirstLoad && this.locale === locale) return;

            this.isFirstLoad = false;
            this.locale = locale;
            LocalStorage.setLanguage(locale);
            await this.reload();
        },
        async reload(){
            try {
                await this.updateI18n();
                await this.loadAppConfig();
            } catch (e) {
                logger.error('[useConfigStore][setLocale]', e)
            }
        },

        async updateI18n() {
            this.isLocaleReady = false;
            try {
                await i18n.loadLanguage();
            } catch (e) {
                logger.error('[useConfigStore][updateI18n]', e)
            } finally {
                this.isLocaleReady = true;
            }
        },

        async loadAppConfig() {
            this.isConfigReady = false;
            try {
                await appConfig.loadConfig();
            } catch (e) {
                logger.error('[useConfigStore][loadAppConfig]', e)
            } finally {
                this.isConfigReady = true;
            }
        },
    },
});