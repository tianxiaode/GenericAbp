import { EntityInterface, isEmpty, logger, splitArray } from "~/libs";
import { useTableBase } from "./useTableBase";
import { useDelay } from "./useDelay";
import { ref } from "vue";

export function useTree<T extends EntityInterface>(api: any) {
    const { delay} = useDelay();
    const isFilter = ref(false);
    const loaded = (records: T[]) => {
        if (isFilter.value) {
            //如果是过滤状态,先判断data.value是否已存在，如果存在，则在records中将其去掉
            const existRecords = data.value.filter((item: any) => records.some((r: any) => r.id === item.id));
            records = records.filter((item: any) => !existRecords.some((r: any) => r.id === item.id));
            //提取同一父id的节点
            const parentIds = records.map((item: any) => item.parentId).filter((item: any, index: number, arr: any) => arr.indexOf(item) === index);
            //获取父并按层次结构排序，以便于按层次插入data.value
            const parents = existRecords.filter((item: any) => parentIds.includes(item.id)).concat(records.filter((item: any) => !parentIds.includes(item.id))).sort((a:any,b:any)=>{
                return a.code.localeCompare(b.code);
            });
            console.log('exist', existRecords);
            console.log('parents', parents);
            parents.forEach((parent: any) => {
                //提取子节点并根据api的排序规则排序
                const sortField = api.sortField;
                const sortOrder = api.sortOrder;
                const children = records.filter((item: any) => item.parentId === parent.id).sort((a:any,b:any)=>{
                    if(sortField){
                        return sortOrder === 'asc'? a[sortField] - b[sortField] : b[sortField] - a[sortField];
                    }
                    return 0;
                });
                insetNodes(parents, children);
            });            
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

    const refresh = async (id: string | number) =>{
        const row: any = data.value.find((item: any) => item.id === id);
        if(row){
            row.expanded = false;
            row.isLoaded = false;
            row.items = null;
            data.value = data.value.filter((item: any) => !item.code.startsWith(row.code+'.'));
            expandNode(row, true);
        }else{
            logger.error('[useTree][refresh]', 'row not found', id);
        }
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
    } = useTableBase(api, loaded, filter);

    const insetNodes = (row: any, appends: any[]) => {
        const id = row.id;
        const index = data.value.findIndex((item: any) => item.id === id);
        //根据index，将data.value分为两个数值
        const [firstData, lastData] = splitArray(data.value, (_: any, i:number)=>{ return i<=index});
        data.value = [...firstData, ...appends, ...lastData];
        row.expanded = true;
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
            row.items?.forEach((item:any) => {
                item.expanded = false;
            });
            data.value = [...firstData, ...row.items, ...lastData];
            return;
        }
        //如果item.expanded为false，说明改节点未展开，需要异步加载子节点数据
        const loadData = await api.getList({parentId: id});
        if(loadData.length === 0){
            row.leaf = true;
            row.expanded = false;
            row.isLoaded = false
            data.value = [...firstData, ...lastData];
            return;
        }
        row.items = loadData;
        data.value = [...firstData, ...row.items, ...lastData];
        row.isLoaded = true;

    }

    const refreshButton = () => {
        return {
            action: (row:any)=> refresh(row.id) , 
            type: 'primary',
            icon: 'fa fa-refresh', 
            title: 'Components.Refresh', order: 5, 
            visible: true
        }
    }
    

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        isFilter,
        refreshButton,
        refresh,
        filter,
        create,
        update,
        remove,
        checkChange,
        sortChange,
        formClose,
        expandNode
    }
}
