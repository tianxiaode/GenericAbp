<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('OpenIddict.Applications')" :filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table v-loading="loading" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :highlight-current-row="true" :default-sort="{ prop: 'clientId', order: 'ascending' }">
            <HighlightColumn :label="t('OpenIddict.Application:ClientId')" prop="clientId" width="full" sortable
                :filterText="filterText" />
            <el-table-column :label="t('OpenIddict.Application:DisplayName')" prop="displayName" width="full" sortable>
            </el-table-column>
            <el-table-column :label="t('OpenIddict.Application:ApplicationType')" prop="applicationType" width="180"
                sortable> </el-table-column>
            <el-table-column :label="t('OpenIddict.Application:ClientType')" prop="clientType" width="180" sortable>
            </el-table-column>
            <ActionColumn width="100" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="api" v-model:loading="loading" />
    </div>

    <ApplicationForm v-if="dialogVisible" v-model="dialogVisible" :entity-id="currentEntityId" @close="formClose" />
    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible">
    </Detail>
    <PermissionsDialog v-bind="permissionsDialogProps" v-if="permissionsDialogVisible"
        v-model:dialog-ref="permissionsDialogRef" v-model="permissionsDialogData" />

</template>

<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import Pagination from '../../toolbars/PaginationToolbar.vue'
import ApplicationForm from './ApplicationForm.vue';
import { ApplicationPermissionsMapToDisplayName, RoleType } from '~/repositories';
import HighlightColumn from '../../table/HighlightColumn.vue';
import { useTable, useRepository, useI18n, useDetail, usePermissionsDialog } from '~/composables';
import ActionColumn from '../../table/ActionColumn.vue';
import PermissionsDialog from '../../dialogs/PermissionsDialog.vue';
import Detail from '../../Detail.vue';
import { capitalize, parseTimeSpanToSeconds } from '~/libs';

const api = useRepository('application');
const { t } = useI18n();

const {
    data, dialogVisible, currentEntityId,
    filterText, loading,
    create, update, remove, filter,
    sortChange, formClose, } = useTable<RoleType>(api);

const toolbarButtons = {
    create: { action: create, isVisible: api.canCreate },
}

const { permissionsDialogData, permissionsDialogProps,
    permissionsDialogRef, permissionsDialogVisible, permissionsShowDialog }
    = usePermissionsDialog(api, 'clientId');


const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(api, [
    { field: 'clientId' },
    { field: 'displayName' },
    { field: 'applicationType' },
    { field: 'clientType' },
    { field: 'consentType' },
    { field: 'clientSecret' },
    { field: 'logoUri' },
    {
        field: 'permissions', type: 'object',
        convert(permissions: string[]) {
            const result = { grantTypes: [], endpoints: [], scopes: [], responseTypes: [] } as any;
            permissions.forEach(permission => {
                let displayName = ApplicationPermissionsMapToDisplayName[permission];
                if (displayName) {
                    displayName = t.value(displayName);
                } else {
                    displayName = capitalize(permission.substring(permission.lastIndexOf(':') + 1));
                }
                if (permission.startsWith('gt:')) {
                    result.grantTypes.push(displayName);
                } else if (permission.startsWith('ept:')) {
                    result.endpoints.push(displayName);
                } else if (permission.startsWith('scp:')) {
                    result.scopes.push(displayName);
                } else if (permission.startsWith('rst:')) {
                    result.responseTypes.push(displayName);
                }
            })
            return result;
        },
        rowItems: [
            { field: 'grantTypes', label: t.value('OpenIddict.GrantTypes'), type: 'list' },
            { field: 'endpoints', label: t.value('OpenIddict.Endpoints'), type: 'list' },
            { field: 'scopes', label: t.value('OpenIddict.Scopes'), type: 'list' },
            { field: 'responseTypes', label: t.value('OpenIddict.ResponseTypes'), type: 'list' }
        ]

    },
    { field: 'redirectUris', type: 'list' },
    { field: 'postLogoutRedirectUris', type: 'list' },
    {
        field: 'settings', type: 'object',
        rowItems: [
            {
                field: 'tkn_lft:act', label: t.value('OpenIddict.Settings:TokenLifetimes.AccessToken'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
            {
                field: 'tkn_lft:auc', label: t.value('OpenIddict.Settings:TokenLifetimes.AuthorizationCode'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
            {
                field: 'tkn_lft:dvc', label: t.value('OpenIddict.Settings:TokenLifetimes.DeviceCode'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
            {
                field: 'tkn_lft:idt', label: t.value('OpenIddict.Settings:TokenLifetimes.IdentityToken'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
            {
                field: 'tkn_lft:reft', label: t.value('OpenIddict.Settings:TokenLifetimes.RefreshToken'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
            {
                field: 'tkn_lft:usrc', label: t.value('OpenIddict.Settings:TokenLifetimes.UserCode'), convert(value: string) {
                    return parseTimeSpanToSeconds(value)
                }
            },
        ]
    }
])

const tableButtons = {
    detail: { action: showDetails },
    edit: { isDisabled: (row: RoleType) => row.isStatic, action: update, isVisible: api.canUpdate },
    delete: { isDisabled: (row: RoleType) => row.isStatic, action: remove, isVisible: api.canDelete },
    permission: { action: permissionsShowDialog, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', isVisible: api.canManagePermissions },
}

</script>
