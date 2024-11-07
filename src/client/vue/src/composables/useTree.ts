import { EntityInterface, isEmpty } from "~/libs";
import { useTableBase } from "./useTableBase";
import { useDelay } from "./useDelay";
import { ref } from "vue";

export function useTree<T extends EntityInterface>(api: any) {
    const { delay} = useDelay();
    const isFilter = ref(false);
    const tableRef = ref<any>(null);
    const loadData = (records: T[]) => {
        if (isFilter.value) {
            data.value = getChildren(records, null);
        } else {
            records.forEach((item: any) => {
                item.hasChildren = !item.leaf;
                tableRef.value?.toggleRowExpansion(item, false);
            });
            data.value = records;
        }
        return;
    };

    const filter = (filter: string) => {
        delay(()=>{
            isFilter.value = !isEmpty(filter);
            filterText.value = filter;
            setTimeout(() => {
                api.filter = filter;                
            }, 50);
        })
    }

    const refresh = async (row: any) =>{
        const id = row.id;
        const data = await api.getList({parentId: id});
        data.forEach((item: any) => {
            item.hasChildren = !item.leaf;
        });
        tableRef.value?.updateKeyChildren(id, data);
    }

    const {
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
        formClose,
    } = useTableBase(api, loadData, filter);

    const getChildren = (data: any, id: any) => {
        let children = [...data.filter((item: any) => item.parentId === id)];
        children.forEach((item: any) => {
            item.children = getChildren(data, item.id);
            tableRef.value?.updateKeyChildren(item.id, item.children);
        })
        return children;
    };
    
    const loadNode = (row: any, _: any, resolve: any) => {
        const parentId = row.id;
        api.getList({ parentId }).then((res: any) => {
            res.forEach((item: any) => {
                item.hasChildren = !item.leaf;
                tableRef.value?.toggleRowExpansion(item, false);
            })
            resolve(res);
        })
    }

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        isFilter,
        tableRef,
        refresh,
        filter,
        create,
        update,
        remove,
        checkChange,
        sortChange,
        formClose,
        loadNode
    }
}
