import {  EntityInterface,  isEmpty,  logger,  replaceMembers, sortBy } from "~/libs";
import { useTableBase } from "./useTableBase";
import { ref } from "vue";
import { useDelay } from "./useDelay";

export function useTree<T extends EntityInterface>(
    api: any,
    defaultSort: Record<string, "ascending" | "descending"> = {}
) {

    let originalRecords = null as any;
    const {delay} = useDelay();
    const idFieldName = api.idFieldName;
    const parentIdFieldName = api.parentIdFieldName;

    const sorts = ref<Record<string, "ascending" | "descending" | null>>({
        ...defaultSort,
    });
    const loaded = (records: T[]) => {
        resetData(records);
    };

    const refresh = async () => {
        const changed = api.currentChanged;
        const row: any = data.value.find((item: any) => item[idFieldName] === changed[idFieldName]);
        if (row) {
            replaceMembers(row, changed);
        } else {
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
            if(isEmpty(text)){
                data.value = [...originalRecords];
                originalRecords = null;
                return;
            }else{
                originalRecords = [...data.value];
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
            data.value = originalRecords.filter(
                (item: any) => !item.code.startsWith(row.code + ".")
            );
            return;
        }
        const loadData = await api.getList({ parentId: id });
        row.expanded = true;
        resetData([...data.value, ...loadData]);
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

    const refreshButton = () => {
        return {
            action: (row: any) => refreshNode(row[idFieldName]),
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
        sorts,
        tableRef,
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
        getLabel,
    };
}
