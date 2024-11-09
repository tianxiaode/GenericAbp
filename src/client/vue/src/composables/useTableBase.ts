import { EntityInterface, LocalStorage } from "../libs";
import { onMounted, onUnmounted, ref, watch } from "vue";
import router from "~/router";
import { useDelay } from "./useDelay";
import { useLabel } from "./useLabel";

export function useTableBase<T extends EntityInterface>(
    api: any,
    loaded: any,
    refresh: any
) {
    const resourceName = api.resourceName;
    const entity = api.entity;
    const data = ref<T[]>([]);
    const filterText = ref("");
    const dialogVisible = ref(false);
    const currentEntityId = ref("");
    const { delay } = useDelay();
    const { getLabel} = useLabel(api);
    const tableRef = ref<any>();

    // Add a new entry
    const create = () => {
        api.currentChanged = false;
        dialogVisible.value = true;
        currentEntityId.value = "";        
    };

    // Edit an existing entry
    const update = async (entity: T) => {
        api.currentChanged = false;
        dialogVisible.value = true;
        currentEntityId.value = entity.id as string;
    };

    // Delete an entry
    const remove = async (entity: T) => {
        await api.delete(entity.id);
    };

    // Filter data based on the filterText
    const filter = (filter: string) => {
        delay(() => {
            filterText.value = filter;
            api.filter = filter;
        });
    };

    const checkChange = (entity: T) => {
        api.update(entity);
    };

    const sortChange = (sort: any) => {
        api.sort(sort.prop, sort.order === "ascending" ? "asc" : "desc");
    };

    const formClose = () => {
        if(api.currentChanged){
            refresh();
        }
    };

    watch(dialogVisible, (isVisible) => {
        if (isVisible) return;
        formClose();
    });

    onMounted(() => {
        if (!api.canRead) {
            LocalStorage.setRedirectPath(router.currentRoute.value.path);
            router.push("/login");
        }
        api.on("load", loaded);
        api.load();
    });

    onUnmounted(() => {
        api.un("load", loaded);
    });

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        tableRef,
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
