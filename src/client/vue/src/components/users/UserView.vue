<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="'用户管理'" @filter="filter" :buttons="toolbarButtons" />

        <!-- 数据展示区域 -->
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'userName', order: 'ascending' }">
            <HighlightColumn label="用户名称" prop="userName" width="full" sortable :filterText="filterText" />
            <el-table-column label="姓名" prop="surname" width="full" sortable></el-table-column>
            <HighlightColumn label="电子邮箱" prop="email" width="full" sortable :filterText="filterText" />
            <el-table-column label="电话" prop="phoneNumber" width="full" sortable></el-table-column>
            <CheckColumn label="启用" prop="isActive" width="80" :filterText="filterText" :checkChange="checkChange">
            </CheckColumn>
            <CheckColumn label="账户锁定" prop="lockoutEnabled" width="100" :filterText="filterText"
                :checkChange="checkChange"></CheckColumn>
            <el-table-column label="已锁定" prop="lockoutEnd" width="180">
                <template #default="scope">
                    {{ formatLockoutDate(scope.row.lockoutEnd) }}
                </template>
            </el-table-column>
            <DateColumn prop="creationTime" label="创建时间" width="180" format="yyyy-MM-dd HH:mm:ss"></DateColumn>
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>

        <!-- 底部分页工具栏 -->
        <Pagination style="margin-top: 10px;" :api="userApi" />
    </div>

    <UserForm v-if="dialogVisible" :title="dialogTitle" v-model:isVisible="dialogVisible"
        :entity-id="currentEntityId" />

    <UserDetail :entity-id="currentEntityId" v-model="detailVisible" />

</template>

<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import Pagination from '../toolbars/PaginationToolbar.vue'
import { UserType, userApi } from '../../repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import CheckColumn from '../table/CheckColumn.vue';
import { useTable } from '../../composables/useTable';
import ActionColumn from '../table/ActionColumn.vue';
import UserForm from './UserForm.vue';
import { formatDate } from '../../libs';
import DateColumn from '../table/DateColumn.vue';
import { ref } from 'vue';
import UserDetail from './UserDetail.vue';

const formatLockoutDate = (date: string | null) => {
    if (!date) {
        return '未锁定';
    }
    return formatDate(date, 'yyyy-MM-dd HH:mm:ss')
};

const {
    data, dialogVisible, dialogTitle, currentEntityId,
    filterText,
    allowedCreate, allowedUpdate, allowedDelete,
    create, update, remove, filter, checkChange,
    sortChange } = useTable<UserType>(userApi, '新增用户', '编辑用户');

const toolbarButtons = {
    create: { action: create, isVisible: allowedCreate }
}

const detailVisible = ref(false);
const showDetail = (row: UserType) => {
    currentEntityId.value = row.id as any;
    detailVisible.value = true;
}

const tableButtons = {
    detail: { 
        action: showDetail, icon: 'ellipsis', title: '详情', order: 1 },
    edit: { action: update, isVisible: allowedUpdate },
    delete: { action: remove, isVisible: allowedDelete },
}




</script>

