<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('OpenIddict.Scopes')" :filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :highlight-current-row="true"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn :label="t('OpenIddict.Scope:Name')" prop="name" width="200'" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('OpenIddict.Scope:DisplayName')" prop="displayName" width="200'" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('OpenIddict.Scope:Description')" prop="description" width="full'" sortable
                :filterText="filterText" />
            <ActionColumn width="90" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" v-model:loading="loading"/>
    </div>

    <ScopeForm v-if="dialogVisible" v-model="dialogVisible" :entity-id="currentEntityId" @close="formClose" />
    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible"></Detail>
</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import ScopeForm from './ScopeForm.vue';
import { RoleType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import { useTable, useRepository, useI18n, useDetail } from '~/composables';
import ActionColumn from '../../table/ActionColumn.vue';
import Detail from '../../Detail.vue';

const api = useRepository('Scope');
const { t } = useI18n();


const {
    data, dialogVisible, currentEntityId,
    filterText,loading,
    create, update, remove, filter,
    sortChange, formClose, } = useTable<RoleType>(api);

const toolbarButtons = {
    create: { action: create, isVisible: api.canCreate },
}

const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(api,[
    { field: 'name'},
    { field: 'displayName'},
    { field: 'description'},
    { field: 'resources', type: 'list'},
])

const tableButtons = {
    detail: { action: showDetails },
    edit: { isDisabled: (row: RoleType) => row.isStatic, action: update, isVisible: api.canUpdate },
    delete: { isDisabled: (row: RoleType) => row.isStatic, action: remove, isVisible: api.canDelete },
}

</script>
