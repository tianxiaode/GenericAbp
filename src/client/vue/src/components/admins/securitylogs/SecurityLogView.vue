<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.SecurityLogs')" @filter="filter" :show-ellipsis-icon="true"
            :buttons="toolbarButtons">
            <template #advanced-search-items>
                <Select :method="api.getAllApplicationNames" v-model="searchValue.applicationName"
                    placeholder="AbpIdentity.SecurityLog:ApplicationName"></Select>
                <Select :method="api.getAllIdentities" v-model="searchValue.identity"
                    placeholder="AbpIdentity.SecurityLog:Identity"></Select>
                <DateRange format="datetime" v-model:start-date="searchValue.startTime"
                    v-model:end-date="searchValue.endTime"></DateRange>
                <Select :method="api.getAllActions" v-model="searchValue.actionName"
                    placeholder="AbpIdentity.SecurityLog:Action"></Select>
                <Select :method="api.getAllUserNames" v-model="searchValue.userName"
                    placeholder="AbpIdentity.SecurityLog:UserName"></Select>
                <Select :method="api.getAllClientIds" v-model="searchValue.clientId"
                    placeholder="AbpIdentity.SecurityLog:ClientId"></Select>
                <Select :method="api.getAllCorrelationIds" v-model="searchValue.correlationId"
                    placeholder="AbpIdentity.SecurityLog:CorrelationId"></Select>
            </template>
        </ActionToolbar>

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'creationTime', order: 'descending' }">
            <DateColumn prop="creationTime" :label="t('AbpIdentity.CreationTime')" width="180"
                format="yyyy-MM-dd HH:mm:ss" sortable>
            </DateColumn>
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ApplicationName')" prop="applicationName" width="full"
                sortable :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:Action')" prop="action" width="full" sortable
                :filterText="filterText">
                <template #default="{ row, prop, filter }">
                    <span v-if="row[prop] === 'LoginSucceeded'" class="text-success">{{ highlightText(row[prop], filter)
                        }}</span>
                    <span v-else-if="row[prop] === 'LoginFailed'" class="text-danger">{{ highlightText(row[prop], filter)
                        }}</span>
                    <span v-else>{{ highlightText(row[prop], filter) }}</span>
                </template>
            </HighlightColumn>
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:Identity')" prop="identity" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:UserName')" prop="userName" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ClientId')" prop="clientId" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('AbpIdentity.SecurityLog:ClientIpAddress')" prop="clientIpAddress" width="120"
                sortable>
            </el-table-column>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" />
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

const { data, filterText, filter, sortChange } = useTable<SecurityLogType>(api);

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
