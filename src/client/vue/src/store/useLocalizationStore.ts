import { defineStore } from 'pinia';
import { globalConfig, i18n } from '../libs';

export const useLocalizationStore = defineStore('localization', {
    state: () => ({
        locale: 'en', // 默认语言
    }),
    actions: {
        setLocale(locale: string) {
            this.locale = locale; // 更新当前语言
            i18n.setLanguage(locale); // 更新 i18n 实例
            globalConfig.setLanguage(locale); // 更新全局配置

        },
        async t(key: string, resource?: string , entity?: string , params?: any) {
            const t = i18n.get.bind(i18n);
            return t(key, resource, entity,  params);
        },
    },
});
