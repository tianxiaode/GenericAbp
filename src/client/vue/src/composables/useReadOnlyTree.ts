import { onMounted } from "vue";
import { useTreeBase } from "./useTreeBase";
import { debounce, isEmpty, mergeArray, SortType } from "~/libs";

export function useReadOnlyTree(api: any, defaultSort: SortType, filterNode: any) {
    let isInitialLoad = true;
    const loadData = async () => {
        const result = await api.getList(filterText.value);
        resetData(mergeArray([[...data.value], result], api.idFieldName));
    };

    const filter = debounce((text: string) => {
        filterText.value = text;
        if (!isEmpty(text)) {
            loadData();
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
        formClose,
        expandNode,
        getLabel,
        resetData,
    } = useTreeBase(
        api,
        defaultSort,
        { loaded: () => {}, refresh: () => {} },
        false,
        filterNode
    );

    onMounted(() => {
        if(isInitialLoad){
            loadData();
            isInitialLoad = false;
        }        
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
        filter,
        checkChange,
        formClose,
        expandNode,
        getLabel,
    };
}
