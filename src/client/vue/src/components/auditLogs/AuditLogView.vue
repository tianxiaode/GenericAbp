<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpAuditLogging.AuditLogs')" @filter="filter" :show-ellipsis-icon="true"
            :buttons="toolbarButtons">
            <template #advanced-search-items>
                <Select :method="api.getAllApplicationNames" v-model="searchValue.applicationName" placeholder="AbpAuditLogging.ApplicationName"></Select>
                <Select :method="api.getAllUrls" v-model="searchValue.url" placeholder="AbpAuditLogging.url"></Select>
                <DateRange format="datetime" v-model:start-date="searchValue.startTime" v-model:end-date="searchValue.endTime"></DateRange>
                <Select :method="api.getAllUserNames" v-model="searchValue.userName" placeholder="AbpAuditLogging.AuditLog:UserName"></Select>
                <Select :method="api.getAllClientIds" v-model="searchValue.clientId" placeholder="AbpAuditLogging.AuditLog:ClientId"></Select>
                <Select :method="api.getAllCorrelationIds" v-model="searchValue.correlationId" placeholder="AbpAuditLogging.AuditLog:CorrelationId"></Select>
            </template>
        </ActionToolbar>

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'executionTime', order: 'descending' }">
            <DateColumn prop="executionTime" :label="t('AbpAuditLogging.ExecutionTime')" width="180"
                format="yyyy-MM-dd HH:mm:ss" sortable>
            </DateColumn>
            <el-table-column :label="t('AbpAuditLogging.ExecutionDuration')" prop="executionDuration" width="120" sortable align="right">
            </el-table-column>
            <HighlightColumn :label="t('AbpAuditLogging.ApplicationName')" prop="applicationName" width="full"
                sortable :filterText="filterText" />
            <HighlightColumn :label="t('AbpAuditLogging.url')" prop="url" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpAuditLogging.UserName')" prop="userName" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('AbpAuditLogging.ClientIpAddress')" prop="clientIpAddress" width="120" sortable>
            </el-table-column>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <AuditLogDetail v-model="detailVisible" :entity-id="detailEntityId" />
  

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" />
    </div>




</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import { AuditLogType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import DateColumn from '../table/DateColumn.vue';
import { useDetail, useI18n, useRepository, useTable } from '~/composables';
import Select from '../forms/Select.vue';
import { ref,watch } from 'vue';
import DateRange from '../forms/DateRange.vue';
import AuditLogDetail from './AuditLogDetail.vue';
import ActionColumn from '../table/ActionColumn.vue';

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

const { data, filterText, filter, sortChange } = useTable<AuditLogType>(api);

const toolbarButtons = {
    create: { isVisible: ()=>false },
}

const { detailVisible, detailEntityId, detailButton  } = useDetail(api.idFieldName);

const tableButtons = {
    // detail: { 
    //     action: showDetail, icon: 'fa fa-ellipsis', title: t.value('Components.Details'), order: 1 },
    edit: { isVisible: ()=>false },
    delete: {  isVisible: ()=>false },
    detail: detailButton,
}






</script>
