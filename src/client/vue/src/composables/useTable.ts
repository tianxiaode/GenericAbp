import { EntityInterface, isGranted } from "../libs";
import { onMounted, onUnmounted, ref } from "vue";

export function useTable<T extends EntityInterface>(api: any) {

    const resourceName = api.resourceName;
    const entity = api.entity;
    const data = ref<T[]>([]);
    const filterText = ref('');
    const dialogVisible = ref(false);
    const currentEntityId = ref('');

    const allowedCreate = () => {
        return isGranted(resourceName, api.entityPlural, 'create');
    }

    const allowedUpdate = () => {
        return isGranted(resourceName, api.entityPlural, 'update');
    }

    const allowedDelete = () => {
        return isGranted(resourceName, api.entityPlural, 'delete');
    }

    const checkPermission = (permission: string) => {
        return ()=>isGranted(resourceName, api.entityPlural, permission);
    }

    // Load data for the table
    const loadData = (records: T[]) => {
        data.value = records;
    };

    // Add a new entry
    const create = () => {
        dialogVisible.value = true;
        currentEntityId.value = '';
    };

    // Edit an existing entry
    const update = async (entity: T) => {
        dialogVisible.value = true;
        currentEntityId.value = entity.id as string;
    };
    

    // Delete an entry
    const remove = async (entity: T) => {
        await api.delete(entity.id);
    };

    // Filter data based on the filterText
    const filter = (filter: string) => {
        filterText.value = filter;
        api.filter = filter;
    };

    const checkChange = (entity:T)=>{
        api.update(entity);
    }

    const sortChange = (sort: any) => {
        console.log(sort);
        api.sort(sort.prop, sort.order === 'ascending' ? 'asc' : 'desc');
    };

    const formClose = () => {
        dialogVisible.value = false;
        api.load(true);
    };

    onMounted(() => {
        api.on('load', loadData);
        api.load();
    });

    onUnmounted(() => {
        api.un('load', loadData);
    });

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        allowedCreate,
        allowedUpdate,
        allowedDelete,
        checkPermission,
        create,
        update,
        remove,
        filter,
        checkChange,
        sortChange,
        formClose
    };
}
