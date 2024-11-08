import { EntityInterface, isEmpty, logger, sortBy, splitArray } from "~/libs";
import { useTableBase } from "./useTableBase";
import { useDelay } from "./useDelay";
import { ref } from "vue";

export function useTree<T extends EntityInterface>(api: any, defaultSort:Record<string,'ascending' | 'descending'> = {}) {
    const { delay } = useDelay();
    const isFilter = ref(false);
    const sorts = ref<Record<string, 'ascending' | 'descending' | null>>({...defaultSort});
    const loaded = (records: T[]) => {
        if (isFilter.value) {
            const originalData = [...data.value];
            //找出records存在于originalData中的数据并移除，然后将records余下的数据插入到originalData中
            records.forEach((record: any) => {
                const index = originalData.findIndex(
                    (item: any) => item.id === record.id
                );
                if (index === -1) {
                    originalData.push(record);
                } else {
                    record.expanded = true;
                }
            });
            data.value = sort(originalData);
        } else {
            data.value = records;
        }
        return;
    };

    const filter = (filter: string) => {
        delay(() => {
            isFilter.value = !isEmpty(filter);
            filterText.value = filter;
            setTimeout(() => {
                api.filter = filter;
            }, 50);
        });
    };

    const refresh = async (id: string | number) => {
        const row: any = data.value.find((item: any) => item.id === id);
        if (row) {
            row.expanded = false;
            row.isLoaded = false;
            row.items = null;
            data.value = data.value.filter(
                (item: any) => !item.code.startsWith(row.code + ".")
            );
            expandNode(row, true);
        } else {
            logger.error("[useTree][refresh]", "row not found", id);
        }
    };

    const sortChange = (field:string, order: 'ascending' | 'descending' | null) => {
        sorts.value[api.sortField] = null;
        sorts.value[field] = order;
        api.sortField = field;
        api.sortOrder = order;
        console.log(api.sortField, api.sortOrder);
        data.value = sort([...data.value]);
    };

    const sort = (data: any[]) => {
        // 首先对整个数据进行排序
        //sortBy(data, api.sortField, api.sortOrder || 'asc');

        // 找到所有根节点
        const roots = data.filter((item: any) => !item.parentId);
        sortBy(roots, api.sortField, api.sortOrder || "asc");
        const result: any[] = [];

        // 遍历根节点，并将其添加到结果中
        roots.forEach((item: any) => {
            result.push(item);
            // 递归获取并插入子节点
            getSortChildren(data, item.id, result);
        });
        console.log(result);
        return result; // 返回最终的平面数据
    };

    // 对指定parentId的子节点进行排序并插入到结果中
    const getSortChildren = (
        data: any[],
        parentId: string | null,
        result: any[]
    ) => {
        // 找到当前parentId的所有子节点
        const children = data.filter((item: any) => item.parentId === parentId);

        // 对子节点进行排序
        sortBy(children, api.sortField, api.sortOrder || "asc");

        // 将子节点添加到结果数组并递归处理其子节点
        children.forEach((child: any) => {
            result.push(child);
            // 递归调用以获取并插入子节点的子节点
            getSortChildren(data, child.id, result);
        });
    };

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
        formClose,
    } = useTableBase(api, loaded, filter);

    const expandNode = async (row: any, expanded: boolean) => {
        const id = row.id;
        const originalRecords = [...data.value];
        if (!expanded) {
            //移除以row.code+'.'开始的数据
            row.expanded = false;
            data.value = originalRecords.filter(
                (item: any) => !item.code.startsWith(row.code + ".")
            );
            return;
        }
        const index = data.value.findIndex((item: any) => item.id === id);
        //根据index，将data.value分为两个数值
        const [firstData, lastData] = splitArray(
            data.value,
            (_: any, i: number) => {
                return i <= index;
            }
        );
        row.expanded = true;
        //根据id，获取子节点
        if (row.isLoaded) {
            //如果item.expanded为true，说明改节点已展开，需要把子节点数据添加到显示列表中
            row.items?.forEach((item: any) => {
                item.expanded = false;
            });
            data.value = [...firstData, ...row.items, ...lastData];
            return;
        }
        //如果item.expanded为false，说明改节点未展开，需要异步加载子节点数据
        const loadData = await api.getList({ parentId: id });
        if (loadData.length === 0) {
            row.leaf = true;
            row.expanded = false;
            row.isLoaded = false;
            data.value = [...firstData, ...lastData];
            return;
        }
        row.items = loadData;
        data.value = [...firstData, ...row.items, ...lastData];
        row.isLoaded = true;
    };

    const refreshButton = () => {
        return {
            action: (row: any) => refresh(row.id),
            type: "primary",
            icon: "fa fa-refresh",
            title: "Components.Refresh",
            order: 5,
            visible: true,
        };
    };

    return {
        resourceName,
        entity,
        data,
        filterText,
        dialogVisible,
        currentEntityId,
        isFilter,
        sorts,
        refreshButton,
        refresh,
        filter,
        create,
        update,
        remove,
        checkChange,
        sortChange,
        formClose,
        expandNode,
    };
}
