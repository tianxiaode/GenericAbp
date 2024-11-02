import {  onMounted, watch } from "vue";
import { useConfigStore } from "../store";
import { storeToRefs } from "pinia";

export function useConfig(callback: Function) {
    const configStore = useConfigStore();
    const { isReady } = storeToRefs(configStore);

    watch(isReady, (newValue) => {
        if (newValue) {
            callback();
        }
    });

    onMounted(() => {
        if (isReady.value) {
            callback();
        }
    });

    return {
        isReady,
    };
}
