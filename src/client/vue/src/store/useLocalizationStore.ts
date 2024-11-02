import { defineStore } from 'pinia';
import { appConfig, i18n,LocalStorage } from '~/libs';

export const useLocalizationStore = defineStore('localization', {
    state: () => ({
        locale: LocalStorage.getLanguage(),
        isReady: false, // 语言是否加载完成
        isFist: true // 是否是第一次加载
    }),
    actions: {
        setReadyState(status: boolean) {
            this.isReady = status; // 更新配置就绪状态
        },
        setLocale(locale: string) {
            if (!this.isFist && this.locale === locale) return; // 相同语言不更新
            this.isFist = false;
            this.locale = locale; // 更新当前语言
            // 更新 i18n 实例
            LocalStorage.setLanguage(locale); // 更新本地存储
            i18n.loadLanguage();
            appConfig.loadConfig(); // 更新全局配置

        }
    }
});
