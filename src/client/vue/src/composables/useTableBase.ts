import { debounce, EntityInterface, LocalStorage } from "../libs";
import { onMounted, onUnmounted, ref, watch } from "vue";
import router from "~/router";
import { useLabel } from "./useLabel";

export function useTableBase<T extends EntityInterface>(
    api: any,
    loaded: any,
    refresh: any,
    autoLoad: boolean = true
) {
    const resourceName = api.resourceName;
    const entity = api.entity;
    const data = ref<T[]>([]);
    const filterText = ref("");
    const dialogVisible = ref(false);
    const currentEntityId = ref("");
    const { getLabel } = useLabel(api);
    const tableRef = ref<any>();
    const loading = ref(false);

    // Add a new entry
    const create = () => {
        api.currentChanged = false;
        dialogVisible.value = true;
        currentEntityId.value = "";
    };

    // Edit an existing entry
    const update = async (entity: any) => {
        api.currentChanged = false;
        dialogVisible.value = true;
        currentEntityId.value = entity[api.idFieldName] as string;
    };

    // Delete an entry
    const remove = async (entity: any) => {
        await api.delete(entity[api.idFieldName]);
    };

    // Filter data based on the filterText
    const filter = debounce((text:string)=>{
        loading.value = true;
        filterText.value = text;
        api.filter = text;
    },500);

    const checkChange = (entity: T) => {
        api.update(entity);
    };

    const sortChange = (sort: any) => {
        api.sort(sort.prop, sort.order === "ascending" ? "asc" : "desc");
    };

    const formClose = () => {
        if (api.currentChanged) {
            refresh();
        }
    };

    watch(dialogVisible, (isVisible) => {
        if (isVisible) return;
        formClose();
    });

    onMounted(() => {
        if (!api.canRead) {
            // 确保路径是安全的
            const currentPath = router.currentRoute.value.path;
            if (currentPath.startsWith("/")) {
                LocalStorage.setRedirectPath(currentPath);
                router.push("/login");
            } else {
                console.error("Invalid path detected:", currentPath);
            }
        }
        if (autoLoad) {
            api.on("load", loaded);
            loading.value = true;
            api.load();
        }
    });
    
    onUnmounted(() => {
        if (autoLoad) {
            api.un("load", loaded);
        }
    });

    return {
        loading,
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
        getLabel,
    };
}
