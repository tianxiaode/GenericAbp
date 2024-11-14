<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpAuditLogging.AuditLogs')" @filter="filter" :show-ellipsis-icon="true"
            :highlight-current-row="true"
            :buttons="toolbarButtons">
            <template #advanced-search-items>
                <Select :method="api.getAllApplicationNames" v-model="searchValue.applicationName" :label="t('AbpAuditLogging.ApplicationName')"></Select>
                <Select :method="api.getAllUrls" v-model="searchValue.url" :label="t('AbpAuditLogging.url')"></Select>
                <DateRange format="datetime" v-model:start-date="searchValue.startTime" v-model:end-date="searchValue.endTime"></DateRange>
                <Select :method="api.getAllUserNames" v-model="searchValue.userName" :label="t(getLabel('UserName'))"></Select>
                <Select :method="api.getAllClientIds" v-model="searchValue.clientId" :label="t(getLabel('ClientId'))"></Select>
                <Select :method="api.getAllCorrelationIds" v-model="searchValue.correlationId" :label="t(getLabel('CorrelationId'))"></Select>
            </template>
        </ActionToolbar>

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'executionTime', order: 'descending' }">
            <DateColumn prop="executionTime" :label="t('AbpAuditLogging.ExecutionTime')" width="180" sortable />
            <el-table-column :label="t('AbpAuditLogging.ExecutionDuration')" prop="executionDuration" width="120" sortable align="right">
            </el-table-column>
            <HighlightColumn :label="t('AbpAuditLogging.ApplicationName')" prop="applicationName" width="full"
                sortable :filterText="filterText" />
            <HighlightColumn :label="t('AbpAuditLogging.url')" prop="url" width="full" sortable
                :filterText="filterText" >
                <template #default="{ row,column, filter }">
                    <span v-html="highlightText(row[column.property], filter)"></span>
                    <HttpMethod :value="row.httpMethod"></HttpMethod>
                    <HttpStatusCode :value="row.httpStatusCode"></HttpStatusCode>
                </template>
            </HighlightColumn>
            <HighlightColumn :label="t('AbpAuditLogging.UserName')" prop="userName" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('AbpAuditLogging.ClientIpAddress')" prop="clientIpAddress" width="180" sortable>
            </el-table-column>
            <ActionColumn width="80" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

  

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" v-model:loading="loading" />
    </div>

    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible"></Detail>



</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import { AuditLogType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import DateColumn from '../../table/DateColumn.vue';
import { useDetail, useI18n, useRepository, useTable } from '~/composables';
import Select from '../../forms/Select.vue';
import { ref,watch } from 'vue';
import DateRange from '../../forms/DateRange.vue';
import Detail from '../../Detail.vue';
import ActionColumn from '../../table/ActionColumn.vue';
import { highlightText } from '~/libs';
import HttpMethod from './HttpMethod.vue';
import HttpStatusCode from './HttpStatusCode.vue';

const { t } = useI18n();
const api = useRepository('AuditLog');
const searchValue = ref<any>({
    applicationName: '',
    url: '',
    startTime: null,
    endTime: null,
    userName: null,
    clientId: null,
    correlationId: null,
});

watch(searchValue.value, (newValue:any) => {
    api.search(newValue, true);
}, { deep: true })

const { loading,data, filterText, filter, sortChange, getLabel } = useTable<AuditLogType>(api);

const toolbarButtons = {
    create: { visible: false },
}

const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(api,[
    { field: 'executionTime'},
    { field: 'executionDuration'},
    { field: 'applicationName'},
    { field: 'url'},
    { field: 'httpMethod'},
    { field: 'httpStatusCode'},
    { field: 'userId'},
    { field: 'userName'},
    { field: 'tenantId'},
    { field: 'tenantName'},
    { field: 'correlationId'},
    { field: 'impersonatorUserId'},
    { field: 'impersonatorUserName'},
    { field: 'impersonatorTenantId'},
    { field: 'impersonatorTenantName'},
    { field: 'clientId'},
    { field: 'clientName'},
    { field: 'browserInfo'},
    { field: 'exceptions', type: 'json' },
    { field: 'comments'},
    { field: 'actions', type: 'json' },
])

const tableButtons = {
    // detail: { 
    //     action: showDetail, icon: 'fa fa-ellipsis', title: t.value('Components.Details'), order: 1 },
    edit: { visible: false },
    delete: {  visible: false },
    detail: { action: showDetails },
}






</script>
