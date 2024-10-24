<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('OpenIddict.Applications')" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn :label="t('OpenIddict.Application:ClientId')" prop="clientId" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('OpenIddict.Application:DisplayName')" prop="displayName" width="full" sortable> </el-table-column>
            <el-table-column :label="t('OpenIddict.Application:ApplicationType')" prop="applicationType" width="180"  sortable> </el-table-column>
            <el-table-column :label="t('OpenIddict.Application:ClientType')" prop="clientType" width="180"  sortable> </el-table-column>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" />
    </div>

    <ApplicationForm v-if="dialogVisible" v-model="dialogVisible" :entity-id="currentEntityId" @close="formClose" />
    <ApplicationDetail v-model="detailVisible" :entity-id="detailEntityId" />
    <PermissionView v-if="permissionVisible" v-model:isVisible="permissionVisible" provider-name="R"
        :provider-key="providerKey" :title="providerKey"></PermissionView>
</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import ApplicationForm from './ApplicationForm.vue';
import { RoleType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import { useTable, useRepository, useI18n, useDetail } from '~/composables';
import ActionColumn from '../table/ActionColumn.vue';
import PermissionView from '../permissions/PermissionView.vue';
import { ref } from 'vue';
import ApplicationDetail from './ApplicationDetail.vue';

const permissionVisible = ref(false);
const providerKey = ref('');

const api = useRepository('application');
const { t } = useI18n();

const openPermissionWindow = (row: RoleType) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.name;
    permissionVisible.value = true;
}


const {
    data, dialogVisible, currentEntityId,
    filterText,
    create, update, remove, filter,
    sortChange, formClose,  } = useTable<RoleType>(api);

const toolbarButtons = {
    create: { action: create, isVisible: api.canCreate },
}

const { detailVisible, detailEntityId, detailButton  } = useDetail(api.idFieldName);

const tableButtons = {
    // detail: { 
    //     action: showDetail, icon: 'fa fa-ellipsis', title: t.value('Components.Details'), order: 1 },
    edit: { isDisabled: (row: RoleType) => row.isStatic, action: update, isVisible: api.canUpdate },
    delete: { isDisabled: (row: RoleType) => row.isStatic, action: remove, isVisible: api.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', isVisible: api.canManagePermissions },
    detail:detailButton,
}

</script>
