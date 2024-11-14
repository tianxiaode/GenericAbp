<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.Roles')" :filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange" :highlight-current-row="true"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn :label="t('AbpIdentity.DisplayName:RoleName')" prop="name" width="full" sortable
                :filterText="filterText" />
            <CheckColumn :label="t('AbpIdentity.DisplayName:IsDefault')" width="120" sortable
                :check-change="checkChange" prop="isDefault"></CheckColumn>
            <CheckColumn :label="t('AbpIdentity.DisplayName:IsPublic')" width="120" sortable :check-change="checkChange"
                prop="isPublic"></CheckColumn>
            <ActionColumn width="100" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" v-model:loading="loading" />
    </div>

    <RoleForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

    <PermissionsDialog v-bind="permissionsDialogProps" v-if="permissionsDialogVisible"
        v-model:dialog-ref="permissionsDialogRef" v-model="permissionsDialogData" />

</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import RoleForm from './RoleForm.vue';
import { RoleType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import CheckColumn from '../../table/CheckColumn.vue';
import { useTable, useRepository, useI18n, usePermissionsDialog } from '~/composables';
import ActionColumn from '../../table/ActionColumn.vue';
import PermissionsDialog from '~/components/dialogs/PermissionsDialog.vue';


const api = useRepository('Role');
const { t } = useI18n();



const {
    data, dialogVisible, currentEntityId,
    filterText,loading,
    create, update, remove, filter, checkChange,
    sortChange, } = useTable<RoleType>(api);

const { permissionsDialogData, permissionsDialogProps,
    permissionsDialogRef, permissionsDialogVisible, permissionsShowDialog }
    = usePermissionsDialog(api, 'name');

const toolbarButtons = {
    create: { action: create, visible: api.canCreate },
}
const tableButtons = {
    detail: { visible: false },
    edit: { disabled: (row: RoleType) => row.isStatic, action: update, visible: api.canUpdate },
    delete: { disabled: (row: RoleType) => row.isStatic, action: remove, visible: api.canDelete },
    permission: { action: permissionsShowDialog, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', visible: api.canManagePermissions },
}

</script>
