import { EntityInterface } from "../libs";
import { useTableBase } from "./useTableBase";

export function useTable<T extends EntityInterface>(api: any) {
    const loaded = (records: T[]) => {
        loading.value = false;
        data.value = records;
        return;
    };

    const refresh = () => {
        loading.value = true;
        api.load();
    };

    const {
        loading,
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        create,
        update,
        remove,
        filter,
        checkChange,
        formClose,
        sortChange,
        getLabel
    } = useTableBase(api, loaded, refresh);
    // Load data for the table

    return {
        loading,
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        create,
        update,
        remove,
        filter,
        checkChange,
        sortChange,
        formClose,
        getLabel
    };
}
