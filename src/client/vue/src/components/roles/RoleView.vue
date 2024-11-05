<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.Roles')" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn :label="t('AbpIdentity.DisplayName:RoleName')" prop="name" width="full" sortable
                :filterText="filterText" />
            <CheckColumn :label="t('AbpIdentity.DisplayName:IsDefault')" width="120" sortable
                :check-change="checkChange" prop="isDefault"></CheckColumn>
            <CheckColumn :label="t('AbpIdentity.DisplayName:IsPublic')" width="120" sortable :check-change="checkChange"
                prop="isPublic"></CheckColumn>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="roleApi" />
    </div>

    <RoleForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

    <PermissionView v-if="permissionVisible" v-model:isVisible="permissionVisible" provider-name="R"
        :provider-key="providerKey" :title="providerKey"></PermissionView>
</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import RoleForm from './RoleForm.vue';
import { RoleType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import CheckColumn from '../table/CheckColumn.vue';
import { useTable, useRepository, useI18n } from '~/composables';
import ActionColumn from '../table/ActionColumn.vue';
import PermissionView from '../permissions/PermissionView.vue';
import { ref } from 'vue';

const permissionVisible = ref(false);
const providerKey = ref('');

const roleApi = useRepository('Role');
const { t } = useI18n();

const openPermissionWindow = (row: RoleType) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.name;
    permissionVisible.value = true;
}


const {
    data, dialogVisible, currentEntityId,
    filterText,
    create, update, remove, filter, checkChange,
    sortChange,  } = useTable<RoleType>(roleApi);

const toolbarButtons = {
    create: { action: create, visible: roleApi.canCreate },
}
const tableButtons = {
    detail:{visible: false},
    edit: { disabled: (row: RoleType) => row.isStatic, action: update, visible: roleApi.canUpdate },
    delete: { disabled: (row: RoleType) => row.isStatic, action: remove, visible: roleApi.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', visible: roleApi.canManagePermissions },
}

</script>
