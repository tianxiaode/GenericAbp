import { storeToRefs } from "pinia";
import { computed, onMounted, ref, watch } from "vue";
import { useLocalizationStore } from "~/store";
import { i18n } from "~/libs";
import zhCn  from 'element-plus/es/locale/lang/zh-cn'
export function useI18n() {
    const localeStore = useLocalizationStore();
    const { isReady, locale } = storeToRefs(localeStore);
    const elementPlusLocale = ref<any>(null);

    watch(locale, async (newLocale) => {
        switch (newLocale) {
            case "en":
                elementPlusLocale.value = await import(
                    "element-plus/es/locale/lang/en"
                );
                break;
            case "zh-TW":
                elementPlusLocale.value = await import(
                    "element-plus/es/locale/lang/zh-tw"
                );
                break;
            default:
                elementPlusLocale.value = zhCn;
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
                return args.join(".");
            }
        };
    });

    onMounted(async () => {
        // 加载默认语言包
        elementPlusLocale.value = zhCn;
    });

    return {
        isReady,
        locale,
        t,
        elementPlusLocale,
    };
}
