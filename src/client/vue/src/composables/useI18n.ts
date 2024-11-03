import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useConfigStore } from "~/store";
import { i18n } from "~/libs";
import zhCn from "element-plus/es/locale/lang/zh-cn";
import en from "element-plus/es/locale/lang/en";
import zhTw from "element-plus/es/locale/lang/zh-tw";
export function useI18n() {
    const localeStore = useConfigStore();
    const { isLocaleReady, locale } = storeToRefs(localeStore);

    const elementPlusLocale = computed(() => {
        switch (locale.value) {
            case "en":
                return en;
            case "zh-TW":
                return zhTw;
            default:
                return zhCn;
        }
    });

    // 动态计算 t 函数，基于 isReady.value 的状态
    const t = computed(() => {
        return (...args: any[]) => {
            // 判断 i18n 是否准备好
            if (isLocaleReady.value) {
                return i18n.get(...args);
            }else{
                return i18n.get(...args);
            } 
        };
    });

    const format = computed(() => {
        if (isLocaleReady.value) {
            console.log('format',i18n.format)
            return i18n.format;
        }else{
            console.log('default format', i18n.defaultFormat)
            return i18n.defaultFormat;
        } 
    });


    return {
        isLocaleReady,
        locale,
        t,
        format,
        elementPlusLocale,
    };
}
