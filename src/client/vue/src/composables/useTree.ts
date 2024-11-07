import { EntityInterface, isEmpty, splitArray } from "~/libs";
import { useTableBase } from "./useTableBase";
import { useDelay } from "./useDelay";
import { ref } from "vue";

export function useTree<T extends EntityInterface>(api: any) {
    const { delay} = useDelay();
    const isFilter = ref(false);
    const loadData = (records: T[]) => {
        if (isFilter.value) {
            data.value = getChildren(records, null);
        } else {
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
        })
        return children;
    };
    
    const loadNode = (row: any, _: any, resolve: any) => {
        const parentId = row.id;
        api.getList({ parentId }).then((res: any) => {
            res.forEach((item: any) => {
                item.hasChildren = !item.leaf;
            })
            resolve(res);
        })
    }

    const expandNode = async (row: any, expanded: boolean) => {
        const id = row.id;
        const originalRecords = [...data.value];
        if(!expanded){
            //移除以row.code+'.'开始的数据
            row.expanded = false;
            data.value = originalRecords.filter((item: any) => !item.code.startsWith(row.code+'.'));
            return;            
        }
        const index = data.value.findIndex((item: any) => item.id === id);
        //根据index，将data.value分为两个数值
        const [firstData, lastData] = splitArray(data.value, (_: any, i:number)=>{ return i<=index});
        row.expanded = true;
        //根据id，获取子节点
        if(row.isLoaded){
            //如果item.expanded为true，说明改节点已展开，需要把子节点数据添加到显示列表中
            row.children?.forEach((item:any) => {
                item.expanded = false;
            });
            data.value = [...firstData, ...row.children, ...lastData];
            return;
        }        
        const loadData = await api.getList({parentId: id});
        row.children = loadData;
        data.value = [...firstData, ...row.children, ...lastData];
        row.isLoaded = true;

    }

    

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        isFilter,
        refresh,
        filter,
        create,
        update,
        remove,
        checkChange,
        sortChange,
        formClose,
        loadNode,
        expandNode
    }
}
