import { ref } from "vue";

interface DetailButton {
    action: (row: any) => void;
    icon: string;
    title: string;
    order: number;
}

export function useDetail(idField: string, buttonConfig?: Partial<DetailButton>) {
    const detailVisible = ref(false);
    const detailEntityId = ref("");

    const showDetails = (row: any) => {
        detailVisible.value = true;
        detailEntityId.value = row[idField];
    };

    const defaultDetailButton: DetailButton = {
        action: showDetails,
        icon: "fa fa-ellipsis",
        title: "Components.Details",
        order: 1,
    };

    const detailButton = { ...defaultDetailButton, ...buttonConfig };

    return {
        detailVisible,
        detailEntityId,
        detailButton
    };
}
