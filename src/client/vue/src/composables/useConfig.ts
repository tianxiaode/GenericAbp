import {  onMounted, watch } from "vue";
import { useConfigStore } from "../store";
import { storeToRefs } from "pinia";

export function useConfig(callback: Function) {
    const configStore = useConfigStore();
    const { isConfigReady } = storeToRefs(configStore);

    watch(isConfigReady, (newValue) => {
        if (newValue) {
            callback();
        }
    });

    onMounted(() => {
        if (isConfigReady.value) {
            callback();
        }
    });

    return {
        isConfigReady,
    };
}
