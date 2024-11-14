<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.Users')" :filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :highlight-current-row="true"
            :default-sort="{ prop: 'userName', order: 'ascending' }">
            <HighlightColumn :label="t('AbpIdentity.DisplayName:UserName')" prop="userName" width="full" sortable :filterText="filterText" >
                <template #default="{ row, filter }">
                    <span class="font-bold" v-html="highlightText(row.userName, filter)"></span>
                    <span class="mx-2">[</span>
                    <span v-html="highlightText(row.name, filter) || '-'"></span>
                    <span class="ml-2" v-html="highlightText(row.surname, filter) || '-'"></span>
                    <span class="ml-2">]</span>
                </template>
            </HighlightColumn>
            <HighlightColumn :label="t('AbpIdentity.DisplayName:Email')" prop="email" width="full" sortable :filterText="filterText" />
            <el-table-column :label="t('AbpIdentity.DisplayName:PhoneNumber')" prop="phoneNumber" width="full" sortable></el-table-column>
            <CheckColumn :label="t('AbpIdentity.DisplayName:IsActive')" prop="isActive" width="80" :filterText="filterText" :checkChange="checkChange">
            </CheckColumn>
            <CheckColumn :label="t('AbpIdentity.DisplayName:LockoutEnabled')"  prop="lockoutEnabled" width="100" :filterText="filterText"
                :checkChange="checkChange"></CheckColumn>
            <el-table-column :label="t('AbpIdentity.Locked')" prop="lockoutEnd" width="180">
                <template #default="scope">
                    {{ formatLockoutDate(scope.row.lockoutEnd) }}
                </template>
            </el-table-column>
            <DateColumn prop="creationTime" :label="t('AbpIdentity.CreationTime')" width="180" />
            <ActionColumn width="100" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="userApi" v-model:loading="Loading" />
    </div>

    <UserForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible"></Detail>

    <PermissionsDialog v-bind="permissionsDialogProps" v-if="permissionsDialogVisible"
        v-model:dialog-ref="permissionsDialogRef" v-model="permissionsDialogData" />

</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import { UserType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import CheckColumn from '../../table/CheckColumn.vue';
import ActionColumn from '../../table/ActionColumn.vue';
import UserForm from './UserForm.vue';
import { formatDate, highlightText } from '~/libs';
import DateColumn from '../../table/DateColumn.vue';
import Detail from '../../Detail.vue';
import { useDetail, useI18n, usePermissionsDialog, useRepository,useTable } from '~/composables';
import PermissionsDialog from '../../dialogs/PermissionsDialog.vue';
import { Loading } from '@element-plus/icons-vue';

const {t, format} = useI18n();
const userApi = useRepository('user');

const formatLockoutDate = (date: string | null) => {
    if (!date) {
        return t.value('AbpIdentity.NotLocked');
    }
    return formatDate(date, format.value.DateTime)
};

const {
    data, dialogVisible, currentEntityId,
    filterText,loading,
    create, update, remove, filter, checkChange,
    sortChange } = useTable<UserType>(userApi);

const toolbarButtons = {
    create: { action: create, isVisible: userApi.canCreate },
}

const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(userApi,[
    { field: 'userName'},
    { field: 'name'},
    { field: 'surname'},
    { field: 'email'},
    { field: 'phoneNumber'},
    { field: 'isActive', type: 'boolean'},
    { field: 'lockoutEnabled', type: 'boolean'},
    { field: 'lockoutEnd', label: 'AbpIdentity.Locked', render:formatLockoutDate},
    { field: 'roleNames', type: 'list', label: 'AbpIdentity.Roles'}
])

const { permissionsDialogData, permissionsDialogProps,
    permissionsDialogRef, permissionsDialogVisible, permissionsShowDialog }
    = usePermissionsDialog(userApi, userApi.idFieldName);

const tableButtons = {
    detail: { action: showDetails },
    edit: { action: update, isVisible: userApi.canUpdate },
    delete: { action: remove, isVisible: userApi.canDelete },
    permission: { action: permissionsShowDialog, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', isVisible: userApi.canManagePermissions }
}




</script>

