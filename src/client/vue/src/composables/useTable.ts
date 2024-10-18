import { EntityInterface, isGranted } from "../libs";
import { onMounted, onUnmounted, ref } from "vue";

export function useTable<T extends EntityInterface>(api: any) {

    const resourceName = api.resourceName;
    const entity = api.entity;
    const data = ref<T[]>([]);
    const filterText = ref('');
    const dialogVisible = ref(false);
    const dialogTitle = ref('');
    const currentEntityId = ref('');

    const allowedCreate = () => {
        return isGranted(resourceName, entity, 'create');
    }

    const allowedUpdate = () => {
        return isGranted(resourceName, entity, 'update');
    }

    const allowedDelete = () => {
        return isGranted(resourceName, entity, 'delete');
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
        api.filter(filter);
    };

    const checkChange = (entity:T)=>{
        api.update(entity);
    }

    const sortChange = (sort: any) => {
        console.log(sort);
        api.setSort(sort.prop, sort.order === 'ascending' ? 'asc' : 'desc');
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
        dialogTitle,
        currentEntityId,
        allowedCreate,
        allowedUpdate,
        allowedDelete,
        create,
        update,
        remove,
        filter,
        checkChange,
        sortChange,
    };
}
