import {  EntityInterface,  isEmpty,  logger,  replaceMembers, sortBy } from "~/libs";
import { useTableBase } from "./useTableBase";
import { onUnmounted, ref } from "vue";
import { useDelay } from "./useDelay";

export function useTree<T extends EntityInterface>(
    api: any,
    defaultSort: Record<string, "ascending" | "descending"> = {}
) {

    const {delay} = useDelay();
    const idFieldName = api.idFieldName;
    const parentIdFieldName = api.parentIdFieldName;
    const currentParent = ref<any>();
    let cache = new Map<string, any>();

    const sorts = ref<Record<string, "ascending" | "descending" | null>>({
        ...defaultSort,
    });
    const loaded = (records: T[]) => {
        if(isEmpty(filterText.value)){
            resetData(records);
        }else{
            //移除records中已存在于data的记录
            const newData = records.filter((item:any) =>!data.value.some( (dataItem:any) => dataItem[idFieldName] === item[idFieldName]));
            resetData([...data.value, ...newData]);
        }
    };

    const refresh = async () => {
        const changed = api.currentChanged;
        const row: any = data.value.find((item: any) => item[idFieldName] === changed[idFieldName]);
        if (row) {
            replaceMembers(row, changed);
        } else {
            const parent: any = findParent(changed);
            if(parent){
                parent.leaf = false;
                parent.expanded = true;
            }
            resetData([...data.value, changed]);
        }
    };

    const sortChange = (
        field: string,
        order: "ascending" | "descending" | null
    ) => {
        sorts.value[api.sortField] = null;
        sorts.value[field] = order;
        api.sortField = field;
        api.sortOrder = order;
        resetData([...data.value])
    };

    const resetData = (records: any[]) => {
        data.value = sort(records);
    }


    const sort = (data: any[]) => {
        // 找到所有根节点
        const roots = data.filter((item: any) => !item[parentIdFieldName]);
        //先对根节点排序
        sortBy(roots, api.sortField, api.sortOrder || "asc");
        const result: any[] = [];

        // 遍历根节点，并将其添加到结果中
        roots.forEach((item: any) => {
            result.push(item);
            // 递归获取并插入子节点
            getSortChildren(data, item, result);
        });
        return result; // 返回最终的平面数据
    };

    // 对指定parentId的子节点进行排序并插入到结果中
    const getSortChildren = (data: any[], parent: any, result: any[]) => {
        // 找到当前parentId的所有子节点
        const children = data.filter(
            (item: any) => item[parentIdFieldName] === parent[idFieldName]
        );

        if (children.length === 0) {
            return;
        }

        parent.expanded = true;
        // 对子节点进行排序
        sortBy(children, api.sortField, api.sortOrder || "asc");

        // 将子节点添加到结果数组并递归处理其子节点
        children.forEach((child: any) => {
            result.push(child);
            // 递归调用以获取并插入子节点的子节点
            getSortChildren(data, child, result);
        });
    };

    const filter = (text: string) => {
        delay(() => {
            filterText.value = text;
            if(!isEmpty(text)){
                api.filter = text;
            }

        });

    };

    const {
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
        checkChange,
        formClose,
        getLabel
    } = useTableBase(api, loaded, refresh);

    const expandNode = async (row: any, expanded: boolean) => {
        const id = row[idFieldName];
        const originalRecords = [...data.value];
        if (!expanded) {
            //移除以row.code+'.'开始的数据
            row.expanded = false;
            const allChild = originalRecords.filter((item: any) => item.code.startsWith(row.code + "."));
            cache.set(id, allChild);
            data.value = originalRecords.filter(
                (item: any) => !item.code.startsWith(row.code + ".")
            );
            return;
        }
        const loadData = await api.getList({ parentId: id });
        row.expanded = true;
        const cachedData = cache.get(id);
        if (!cachedData) {
            resetData([...data.value,  ...loadData]);
            return;
        }            
        //移除cacheData中已存在于loadData的记录
        const newData = cachedData.filter((item: any) => !loadData.some( (dataItem:any) => dataItem[idFieldName] === item[idFieldName]));
        cache.delete(id);
        resetData([...data.value, ...newData, ...loadData]);

    };

    const refreshNode = (id: string | number) =>{
        const row:any = data.value.find((item: any) => item[idFieldName] === id);
        if (row) {
            //移除row的子节点，然后调用expandNode展开
            row.expanded = false;
            data.value = data.value.filter(
                (item: any) => !item[parentIdFieldName] || item[parentIdFieldName] !== id
            );
            expandNode(row, true);
        }else{
            logger.error(`Can not find row with id ${id}`);
        }
    }


    const treeCreate = (row?: any) => {
        currentParent.value = row;
        create();
    }

    const treeUpdate = (row: any) => {
        currentParent.value = findParent(row);
        update(row);
    }

    const treeRemove = async (row: any) => {
        await remove(row);
        data.value = data.value.filter((item: any) => !item.code.startsWith(row.code));
    }

    const findParent = (row: any) => {
        return data.value.find((item: any) => item[idFieldName] === row[parentIdFieldName]);
    }

    const buttons = () => {
        return {
            refresh:{
                action: (row: any) => refreshNode(row[idFieldName]),
                type: "primary",
                icon: "fa fa-refresh",
                title: "Components.Refresh",
                order: 5,
                visible: true,    
            },
            create: {
                action: (row:any) => treeCreate(row),
                type: 'success',
                icon: 'fa fa-plus',
                title: 'Components.New',
                order: 10,
                visible: api.canCreate,
            },
            move:{
                action: () => {},
                type: 'primary',
                icon: 'fa fa-arrows',
                title: 'Components.MoveTo',
                order: 250,
                visible: api.canUpdate,
            },
            copy:{
                action: () => {},
                type: 'primary',
                icon: 'fa fa-copy',
                title: 'Components.CopyTo',
                order: 255,
                visible: api.canUpdate,
            }
        };
    };

    onUnmounted(()=>{
        cache.clear();
        cache = new Map<string, any>();
    })

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        currentParent,
        sorts,
        tableRef,
        buttons: buttons(),
        refresh,
        filter,
        create: treeCreate,
        update: treeUpdate,
        remove: treeRemove,
        checkChange,
        sortChange,
        formClose,
        expandNode,
        getLabel,
    };
}
