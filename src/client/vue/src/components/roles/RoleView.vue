<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="'角色管理'" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'name', order: 'ascending' }">
            <HighlightColumn label="名称" prop="name" width="full" sortable :filterText="filterText" />
            <CheckColumn label="默认" width="80" sortable :check-change="checkChange" prop="isDefault"></CheckColumn>
            <CheckColumn label="默认" width="80" sortable :check-change="checkChange" prop="isPublic"></CheckColumn>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="roleApi" />
    </div>

    <RoleForm v-if="dialogVisible" :title="dialogTitle" v-model:isVisible="dialogVisible"
        :entity-id="currentEntityId" />

    <PermissionView v-if="permissionVisible" v-model:isVisible="permissionVisible" provider-name="R" :provider-key="providerKey"></PermissionView>
</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import RoleForm from './RoleForm.vue';
import { RoleType, roleApi } from '../../repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import CheckColumn from '../table/CheckColumn.vue';
import { useTable } from '../../composables/useTable';
import ActionColumn from '../table/ActionColumn.vue';
import PermissionView from '../permissions/PermissionView.vue';
import { ref } from 'vue';

const permissionVisible = ref(false);
const providerKey = ref('');

const openPermissionWindow = (row: RoleType) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.name;
    permissionVisible.value = true;
}


const {
    data, dialogVisible, dialogTitle, currentEntityId,
    filterText,
    allowedCreate, allowedUpdate, allowedDelete,
    create, update, remove, filter, checkChange,
    sortChange } = useTable<RoleType>(roleApi, '新增角色', '编辑角色');

const toolbarButtons = {
    create: { action: create, isVisible: allowedCreate }
}

const tableButtons = {
    edit: { isDisabled: (row: RoleType) => row.isStatic, action: update , isVisible: allowedUpdate },
    delete: { isDisabled: (row: RoleType) => row.isStatic, action: remove , isVisible: allowedDelete },
    permission:{ action: openPermissionWindow, icon: 'lock', title: '权限定义', order:300, type: 'primary', isVisible: allowedUpdate }
}

</script>

