<template>
    <el-pagination :current-page="currentPage" :page-size="pageSize" :page-sizes="pageSizes"
        layout="prev, pager, next, slot, jumper, ->, sizes, total, " :total="totalRecords" @size-change="pageSizeChange"
        @current-change="pageChange">
        <el-button key="1" type="primary" @click="refresh" style="margin-left: 10px;">
            <i class="fa fa-refresh primary"></i>
        </el-button>
    </el-pagination>
</template>

<script setup lang="ts">
import { usePagination } from '~/composables';

const props = defineProps({
    api: { type: Object, required: true }
});

const { currentPage, pageSize, pageSizes, totalRecords, pageChange, pageSizeChange, refresh } = usePagination(props.api);



</script>
