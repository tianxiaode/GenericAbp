import { onMounted, onUnmounted, ref } from "vue";

export function usePagination(api: any) {
    const currentPage = ref(1);
    const pageSizes = ref(api.getPageSizes());
    const pageSize = ref(api.getPageSize());
    const totalRecords = ref(0);

    // Load data for the table
    const loadData = async (_: any, total: number) => {
        totalRecords.value = total;
    };

    const pageChange = (page: number) => {
        currentPage.value = page;
        api.setPage(page);
    };

    const pageSizeChange = (size: number) => {
        pageSize.value = size;
        api.setPageSize(size);
    };


    const refresh = () => {
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
