import { defineStore } from 'pinia';
import { appConfig, i18n } from '~/libs';

export const useLocalizationStore = defineStore('localization', {
    state: () => ({
        locale: 'en', // 默认语言
        isReady: false, // 语言是否加载完成
    }),
    actions: {
        setReadyState(status: boolean) {
            this.isReady = status; // 更新配置就绪状态
        },
        setLocale(locale: string) {
            this.locale = locale; // 更新当前语言
            // 更新 i18n 实例
            i18n.setLanguage(locale);
            appConfig.setLanguage(locale); // 更新全局配置

        }
    }
});
