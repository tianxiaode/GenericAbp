<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.SecurityLogs')" :filter="filter" :show-ellipsis-icon="true"
            :buttons="toolbarButtons">
            <template #advanced-search-items>
                <Select :method="api.getAllApplicationNames" v-model="searchValue.applicationName"
                    :label="t(getLabel('ApplicationName'))"></Select>
                <Select :method="api.getAllIdentities" v-model="searchValue.identity"
                :label="t(getLabel('Identity'))"></Select>
                <DateRange format="datetime" v-model:start-date="searchValue.startTime"
                    v-model:end-date="searchValue.endTime"></DateRange>
                <Select :method="api.getAllActions" v-model="searchValue.actionName"
                :label="t(getLabel('actionName'))"></Select>
                <Select :method="api.getAllUserNames" v-model="searchValue.userName"
                :label="t(getLabel('userName'))"></Select>
                <Select :method="api.getAllClientIds" v-model="searchValue.clientId"
                :label="t(getLabel('clientId'))"></Select>
                <Select :method="api.getAllCorrelationIds" v-model="searchValue.correlationId"
                :label="t(getLabel('correlationId'))"></Select>
            </template>
        </ActionToolbar>

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :highlight-current-row="true"
            :default-sort="{ prop: 'creationTime', order: 'descending' }">
            <DateColumn prop="creationTime" :label="t('AbpIdentity.CreationTime')" width="180" sortable />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ApplicationName')" prop="applicationName" width="full"
                sortable :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:Action')" prop="action" width="full" sortable
                :filterText="filterText">
                <template #default="{ row, column, filter }">
                    <span 
                        :class="row[column.property] === 'LoginSucceeded' ? 'text-success' : 'text-danger'"
                        v-html="highlightText(row[column.property], filter)"></span>
                </template>
            </HighlightColumn>
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:Identity')" prop="identity" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:UserName')" prop="userName" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ClientId')" prop="clientId" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('AbpIdentity.SecurityLog:ClientIpAddress')" prop="clientIpAddress" width="180"
                sortable>
            </el-table-column>
            <ActionColumn width="90" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" v-model:loading="loading" />
    </div>

    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible">
    </Detail>



</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import { SecurityLogType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import DateColumn from '../../table/DateColumn.vue';
import { useDetail, useI18n, useRepository, useTable } from '~/composables';
import Select from '../../forms/Select.vue';
import { ref, watch } from 'vue';
import DateRange from '../../forms/DateRange.vue';
import Detail from '../../Detail.vue';
import ActionColumn from '~/components/table/ActionColumn.vue';
import { highlightText } from '~/libs';

const { t } = useI18n();
const api = useRepository('SecurityLog');
const searchValue = ref<any>({
    applicationName: '',
    identity: '',
    startTime: null,
    endTime: null,
    actionName: null,
    userName: null,
    clientId: null,
    correlationId: null,
});

watch(searchValue.value, (newValue: any) => {
    api.search(newValue, true);
}, { deep: true })

const { data, filterText,loading, filter, sortChange, getLabel } = useTable<SecurityLogType>(api);

const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(api, [
    { field: 'applicationName' },
    { field: 'action' },
    { field: 'identity' },
    { field: 'userId' },
    { field: 'userName' },
    { field: 'tenantId' },
    { field: 'tenantName' },
    { field: 'tenantId' },
    { field: 'correlationId' },
    { field: 'clientIpAddress' },
    { field: 'browserInfo' }
])


const toolbarButtons = {
    create: { visible: false },
}

const tableButtons = {
    // detail: { 
    //     action: showDetail, icon: 'fa fa-ellipsis', title: t.value('Components.Details'), order: 1 },
    edit: { visible: false },
    delete: { visible: false },
    detail: { action: showDetails },
}







</script>
