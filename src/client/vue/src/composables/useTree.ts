import {
    debounce,
    EntityInterface,
    findItemInArray,
    isEmpty,
    mergeArray,
    replaceMembers,
    SortType,
} from "~/libs";
import { onUnmounted } from "vue";
import { useTreeBase } from "./useTreeBase";

export function useTree<T extends EntityInterface>(
    api: any,
    defaultSort: SortType = {} as SortType
) {
    const idFieldName = api.idFieldName;
    let cache = new Map<string, any>();

    const loaded = (records: T[]) => {
        loading.value = false;
        if (isEmpty(filterText.value)) {
            resetData(records);
        } else {
            resetData(mergeArray([data.value, records], idFieldName));
        }
    };

    const refresh = async () => {
        const changed = api.currentChanged;
        const row: any = findItemInArray(data.value, idFieldName, changed)
        if (row) {
            replaceMembers(changed, row);
        } else {
            const parent: any = findParent(changed);
            if (parent) {
                parent.leaf = false;
                parent.expanded = true;
            }
            resetData([...data.value, changed]);
        }
    };

    const moveOrCopyRefresh = async (
        action: string,
        source: any,
        target: any
    ) => {
        action === "move"
            ? moveRefresh(source, target)
            : refreshTarget([...data.value], target);
    };

    const moveRefresh = async (source: any, target: any) => {
        console.log(source)
        //移除sourcenode及其子节点
        let originalRecords = data.value.filter(
            (item: any) => !item.code.startsWith(source.code)
        );
        const sourceParent: any = findParent(source);
        if (sourceParent) {
            sourceParent.leaf = !originalRecords.some(
                (item: any) =>
                    item[api.parentIdFieldName] ===
                    sourceParent[api.idFieldName]
            );
        }
        await refreshTarget(originalRecords, target);
    };

    const refreshTarget = async (original: any, target: any) => {
        const targetNode: any = data.value.find(
            (item: any) => item[idFieldName] === target[idFieldName]
        );
        if (targetNode) {
            targetNode.leaf = false;

            const newData = await api.getList({
                [api.parentIdFieldName]: target[idFieldName],
            });
            resetData(mergeArray([original, newData], idFieldName));
        }
    };

    const sortChange = (
        field: string
    ) => {
        if(treeSort.value.prop === field){
            const order = treeSort.value.order === "ascending" ? "descending" : "ascending";
            treeSort.value.order = order;
        }else{
            treeSort.value.prop = field;
            treeSort.value.order = "ascending";
        }
        resetData([...data.value]);
    };

    const filter = debounce((text: string) => {
        filterText.value = text;
        if (!isEmpty(text)) {
            loading.value = true;
            api.filter = text;
        }
    }, 500);

    const {
        loading,
        resourceName,
        entity,
        data,
        treeSort,
        filterText,
        dialogVisible,
        currentEntityId,
        currentParent,
        tableRef,
        checkChange,
        create,
        update,
        remove,
        refreshNode,
        findParent,
        formClose,
        expandNode,
        getLabel,
        resetData,
    } = useTreeBase(api, defaultSort, { loaded, refresh });

    const treeCreate = (row?: any) => {
        currentParent.value = row;
        create();
    };

    const treeUpdate = (row: any) => {
        currentParent.value = findParent(row);
        update(row);
    };

    const treeRemove = async (row: any) => {
        await remove(row);
        data.value = data.value.filter(
            (item: any) => !item.code.startsWith(row.code)
        );
    };

    const buttons = () => {
        return {
            refresh: {
                action: (row: any) => refreshNode(row[idFieldName]),
                type: "primary",
                icon: "fa fa-refresh",
                title: "Components.Refresh",
                order: 5,
                visible: true,
            },
            create: {
                action: (row: any) => treeCreate(row),
                type: "success",
                icon: "fa fa-plus",
                title: "Components.New",
                order: 10,
                visible: api.canCreate,
            },
        };
    };

    onUnmounted(() => {
        cache.clear();
        cache = new Map<string, any>();
    });

    return {
        loading,
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        currentParent,
        treeSort,
        tableRef,
        buttons: buttons(),
        refresh,
        moveOrCopyRefresh,
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
