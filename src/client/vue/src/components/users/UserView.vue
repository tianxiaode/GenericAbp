<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('AbpIdentity.Users')" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
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
            <DateColumn prop="creationTime" :label="t('AbpIdentity.CreationTime')" width="180" format="yyyy-MM-dd HH:mm:ss"></DateColumn>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="userApi" />
    </div>

    <UserForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible"></Detail>

    <PermissionView v-if="permissionVisible" v-model:isVisible="permissionVisible" provider-name="U"
        :provider-key="providerKey" :title="permissionViewTitle"></PermissionView>

</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import { UserType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import CheckColumn from '../table/CheckColumn.vue';
import ActionColumn from '../table/ActionColumn.vue';
import UserForm from './UserForm.vue';
import { formatDate, highlightText } from '~/libs';
import DateColumn from '../table/DateColumn.vue';
import { ref } from 'vue';
import Detail from '../Detail.vue';
import { useDetail, useI18n, useRepository,useTable } from '~/composables';
import PermissionView from '../permissions/PermissionView.vue';

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
    filterText,
    create, update, remove, filter, checkChange,
    sortChange } = useTable<UserType>(userApi);

const toolbarButtons = {
    create: { action: create, isVisible: userApi.canCreate },
}

const permissionVisible = ref(false);
const providerKey = ref('');
const permissionViewTitle = ref('');
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

const openPermissionWindow = (row: any) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.id;
    permissionViewTitle.value = row.userName;
    permissionVisible.value = true;
}

const tableButtons = {
    detail: { action: showDetails },
    edit: { action: update, isVisible: userApi.canUpdate },
    delete: { action: remove, isVisible: userApi.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', isVisible: userApi.canManagePermissions }
}




</script>

