<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.SecurityLogs')" @filter="filter" :show-ellipsis-icon="true"
            :buttons="toolbarButtons">
            <template #advanced-search-items>
                <Select :method="api.getAllApplicationNames" v-model="searchValue.applicationName" placeholder="AbpIdentity.SecurityLog:ApplicationName"></Select>
                <Select :method="api.getAllIdentities" v-model="searchValue.identity" placeholder="AbpIdentity.SecurityLog:Identity"></Select>
                <DateRange format="datetime" v-model:start-date="searchValue.startTime" v-model:end-date="searchValue.endTime"></DateRange>
                <Select :method="api.getAllActions" v-model="searchValue.actionName" placeholder="AbpIdentity.SecurityLog:Action"></Select>
                <Select :method="api.getAllUserNames" v-model="searchValue.userName" placeholder="AbpIdentity.SecurityLog:UserName"></Select>
                <Select :method="api.getAllClientIds" v-model="searchValue.clientId" placeholder="AbpIdentity.SecurityLog:ClientId"></Select>
                <Select :method="api.getAllCorrelationIds" v-model="searchValue.correlationId" placeholder="AbpIdentity.SecurityLog:CorrelationId"></Select>
            </template>
        </ActionToolbar>

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'creationTime', order: 'descending' }">
            <el-table-column type="expand">
                <template #default="props">
                    <el-descriptions :column="1" border class="mx-2">
                    <el-descriptions-item :label="t('AbpIdentity.SecurityLog:UserId')">
                        {{ props.row.userId  || '-' }}
                    </el-descriptions-item>
                    <el-descriptions-item :label="t('AbpIdentity.SecurityLog:TenantId')">
                        {{ props.row.tenantId || '-' }}
                    </el-descriptions-item>
                    <el-descriptions-item :label="t('AbpIdentity.SecurityLog:TenantName')">
                        {{ props.row.tenantName || '-' }}
                    </el-descriptions-item>
                    <el-descriptions-item :label="t('AbpIdentity.SecurityLog:CorrelationId')">
                        {{ highlightText(props.row.correlationId, filterText) || '-' }}
                    </el-descriptions-item>
                    <el-descriptions-item :label="t('AbpIdentity.SecurityLog:BrowserInfo')">
                        {{ props.row.browserInfo || '-' }}
                    </el-descriptions-item>
                    </el-descriptions>
                </template>
            </el-table-column>
            <DateColumn prop="creationTime" :label="t('AbpIdentity.CreationTime')" width="180"
                format="yyyy-MM-dd HH:mm:ss" sortable>
            </DateColumn>
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ApplicationName')" prop="applicationName" width="full"
                sortable :filterText="filterText" />
                <HighlightColumn :label="t('AbpIdentity.SecurityLog:Action')" prop="action" width="full"
                sortable :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:Identity')" prop="identity" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:UserName')" prop="userName" width="full" sortable
                :filterText="filterText" />
            <HighlightColumn :label="t('AbpIdentity.SecurityLog:ClientId')" prop="clientId" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('AbpIdentity.SecurityLog:ClientIpAddress')" prop="clientIpAddress" width="120" sortable>
            </el-table-column>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" />
    </div>




</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import { SecurityLogType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import DateColumn from '../table/DateColumn.vue';
import { useI18n, useRepository, useTable } from '~/composables';
import { highlightText } from '~/libs';
import Select from '../forms/Select.vue';
import { ref,watch } from 'vue';
import DateRange from '../forms/DateRange.vue';

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

watch(searchValue.value, (newValue:any) => {
    api.search(newValue, true);
}, { deep: true })

const { data, filterText, filter, sortChange } = useTable<SecurityLogType>(api);

const toolbarButtons = {
    create: { isVisible: ()=>false },
}







</script>
