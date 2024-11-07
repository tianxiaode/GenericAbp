import { EntityInterface,  LocalStorage } from "../libs";
import { onMounted, onUnmounted, ref,watch } from "vue";
import router from "~/router";

export function useTableBase<T extends EntityInterface>(api: any, loadData: any, filter: any) {

    const resourceName = api.resourceName;
    const entity = api.entity;
    const data = ref<T[]>([]);
    const filterText = ref('');
    const dialogVisible = ref(false);
    const currentEntityId = ref('');
    
   
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


    const checkChange = (entity:T)=>{
        api.update(entity);
    }

    const sortChange = (sort: any) => {
        api.sort(sort.prop, sort.order === 'ascending' ? 'asc' : 'desc');
    };

    const formClose = () => {
        api.load();
    };

    watch(dialogVisible, (isVisible) => {
        if(isVisible) return;
        formClose();
    })

    onMounted(() => {
        if(!api.canRead){
            LocalStorage.setRedirectPath(router.currentRoute.value.path);
            router.push('/login');
        }
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
        create,
        update,
        remove,
        checkChange,
        sortChange,
        formClose
    };
}
