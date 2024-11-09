import { ref } from "vue";

export function useSelection() {
    const selected = ref<any[]>([]);

    const handleSelectionChange = (items: any[]) => {
        selected.value = items;
    }

    return {
        selected,
        handleSelectionChange
    }
}