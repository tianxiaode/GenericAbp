<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpTenantManagement.Tenants')" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :highlight-current-row="true"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn :label="t('AbpTenantManagement.DisplayName:TenantName')" prop="name" width="full" sortable
                :filterText="filterText" />
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" />
    </div>

    <TenantForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import TenantForm from './TenantForm.vue';
import { TenantType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import { useTable, useRepository, useI18n } from '~/composables';
import ActionColumn from '../../table/ActionColumn.vue';


const api = useRepository('Tenant');
const { t } = useI18n();


const {
    data, dialogVisible, currentEntityId,
    filterText,
    create, update, remove, filter, 
    sortChange,  } = useTable<TenantType>(api);

const toolbarButtons = {
    create: { action: create, isVisible: api.canCreate },
}
const tableButtons = {
    detail:{visible:false},
    edit: {  action: update, isVisible: api.canUpdate },
    delete: { action: remove, isVisible: api.canDelete }
}

</script>
