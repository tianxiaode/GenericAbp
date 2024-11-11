import { storeToRefs } from "pinia";
import { onMounted, ref, watch } from "vue";
import { appConfig, LanguageType } from "~/libs";
import { useConfigStore } from "~/store";

export function useMultilingual() {
    const configStore = useConfigStore();
    const { isConfigReady } = storeToRefs(configStore);
    const languages = ref<LanguageType[]>([]);
    const currentLanguage = ref<LanguageType>();
    const defaultRowItems = ref<any[]>([]);

    const updateLanguages = () => {
        languages.value = appConfig.languages;
        currentLanguage.value = appConfig.currentCulture;
        defaultRowItems.value = languages.value.map(lang => {
            return {
                field: lang.cultureName,
                label: lang.displayName,
            }
        });
    };

    const changeLanguage = (cultureName: string) => {
        configStore.setLocale(cultureName);
    }

    watch(isConfigReady, (newValue) => {
        if (newValue) {
            updateLanguages();
        }
    });


    onMounted(() => {
        if (isConfigReady.value) {
            updateLanguages();
        }
    });

    return {
        languages,
        currentLanguage,
        defaultRowItems,
        changeLanguage
    };
}
