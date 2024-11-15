import { findItemInArray, logger, mergeArray, sortBy, SortType } from "~/libs";
import { useTableBase } from "./useTableBase";
import { onUnmounted, ref } from "vue";

export function useTreeBase(
    api: any,
    defaultSort: SortType = {} as SortType,
    config: any = {},
    autoLoad: boolean = true,
    filterNode?: any
) {
    const idFieldName = api.idFieldName;
    const parentIdFieldName = api.parentIdFieldName;
    const currentParent = ref<any>();
    let cache = new Map<string, any>();

    const treeSort = ref<SortType>({
        ...defaultSort
    } as SortType);

    const resetData = (records: any[]) => {
        data.value = sort(records);
    };


    const sort = (data: any[]) => {
        // 找到所有根节点
        const roots = data.filter((item: any) => !item[parentIdFieldName]);
        //先对根节点排序
        sortBy(roots, treeSort.value.prop, treeSort.value.order || 'ascending');
        let result: any[] = [];

        // 遍历根节点，并将其添加到结果中
        roots.forEach((item: any) => {
            result.push(item);
            // 递归获取并插入子节点
            getSortChildren(data, item, result);
        });
        if (filterNode) {
            result = result.filter(
                (m: any) => !m.code.startsWith(filterNode.code)
            );
        }
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
        sortBy(children, treeSort.value.prop, treeSort.value.order || 'ascending');

        // 将子节点添加到结果数组并递归处理其子节点
        children.forEach((child: any) => {
            result.push(child);
            // 递归调用以获取并插入子节点的子节点
            getSortChildren(data, child, result);
        });
        return children.length;
    };

    const {
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
        checkChange,
        formClose,
        getLabel,
    } = useTableBase(api, config.loaded, config.refresh, autoLoad);

    const expandNode = async (row: any, expanded: boolean) => {
        const id = row[idFieldName];
        const originalRecords: any = [...data.value];
        if (!expanded) {
            //移除以row.code+'.'开始的数据
            row.expanded = false;
            const allChild = findAllChildren(originalRecords, row);
            cache.set(id, allChild);
            data.value = removeAllChildren(originalRecords, row);
            return;
        }
        row.loading = true;
        try {
            const loadData = await api.getList({ parentId: id });
            row.expanded = true;
            const cachedData: any = cache.get(id);
            if (!cachedData) {
                resetData(mergeArray([originalRecords, loadData], idFieldName));
                return;
            }
            cache.delete(id);
            resetData(
                mergeArray([originalRecords, cachedData, loadData], idFieldName)
            );
        } catch (error) {
            logger.error("[useTree][expandNode] error:", error);
        } finally {
            row.loading = false;
        }
    };

    const refreshNode = (id: string | number) => {
        const row: any = findNodeById(id);
        if (row) {
            //移除row的子节点，然后调用expandNode展开
            row.expanded = false;
            data.value = removeAllChildren([...data.value], row);
            expandNode(row, true);
        } else {
            logger.error(`Can not find row with id ${id}`);
        }
    };

    const findNodeById = (id: string | number) => {
        return data.value.find((item: any) => item[idFieldName] === id);
    };

    const findParent = (row: any) => {
        return findItemInArray(data.value, idFieldName, row[parentIdFieldName]);
    };

    const findAllChildren = (data: any[], row: any) => {
        return data.filter((item: any) => item.code.startsWith(row.code + "."));
    };

    const removeAllChildren = (data: any[], row: any) => {
        return data.filter(
            (item: any) => !item.code.startsWith(row.code + ".")
        );
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
    };
}
