import { onMounted, onUnmounted, ref } from "vue";

export function usePagination(api: any, loading:any) {
    const currentPage = ref(1);
    const pageSizes = ref(api.pageSizes);
    const pageSize = ref(api.pageSize);
    const totalRecords = ref(0);

    // Load data for the table
    const loadData = async (_: any, total: number) => {
        totalRecords.value = total;
    };

    const pageChange = (page: number) => {
        loading.value = true;
        currentPage.value = page;
        api.page = page;
    };

    const pageSizeChange = (size: number) => {
        loading.value = true;
        pageSize.value = size;
        api.pageSize = size;
    };


    const refresh = () => {
        loading.value = true;
        api.load();
    }

    onMounted(() => {
        api.on('load', loadData);
    });

    onUnmounted(() => {
        api.un('load', loadData);
    });

    return {
        currentPage,
        pageSizes,
        pageSize,
        totalRecords,
        pageChange,
        pageSizeChange,
        refresh
    };
}
