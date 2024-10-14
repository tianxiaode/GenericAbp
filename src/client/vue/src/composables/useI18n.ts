import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useLocalizationStore } from "~/store";
import { i18n } from "~/libs";

export function useI18n() {
    const localeStore = useLocalizationStore();
    const { isReady, locale } = storeToRefs(localeStore);

    const elementPlusLocale = computed(async () => {
        switch (locale.value) {
            case 'en':
                return await import('element-plus/es/locale/lang/en');
            case 'zh-TW':
                return await import('element-plus/es/locale/lang/zh-tw');
            default:
                return await import('element-plus/es/locale/lang/zh-cn');
        }
    });

    // 动态计算 t 函数，基于 isReady.value 的状态
    const t = computed(() => {
        return (...args: any[]) => {
            // 判断 i18n 是否准备好
            if (isReady.value) {
                return i18n.get(...args);
            } else {
                // 如果尚未准备好，可以返回 key 或者其他占位符
                return args.join('.');
            }
        };
    });



    return {
        isReady,
        locale,
        t,
        elementPlusLocale
    };
}
