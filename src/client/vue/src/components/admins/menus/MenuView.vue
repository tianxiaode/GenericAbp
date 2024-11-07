<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('MenuManagement.Menus')" @filter="filter" :buttons="toolbarButtons" >
            <template #action-items>
                <Select v-model="groupName" :method="api.getAllGroups" placeholder="MenuManagement.Menu:GroupName" 
                style="order:350;width: 200px;"
                ></Select>
            </template>
        </ActionToolbar>
        <el-table :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'order', order: 'ascending' }" row-key="id"
            :tree-props="{ children: 'items' }"
            >
            <TreeColumn prop="name" :label="t('MenuManagement.Menu:Name')" width="full" sortable :filterText="filterText" 
                :expand="expandNode"
            />
            <el-table-column :label="t('MenuManagement.Menu:GroupName')" prop="groupName" width="full"
                sortable></el-table-column>
            <el-table-column :label="t('MenuManagement.Menu:Icon')" prop="icon" width="100" sortable>
                <template #default="scope">
                    <i v-if="scope.row.icon" :class="scope.row.icon"></i>
                    <span v-else>-</span>
                </template>
            </el-table-column>
            <el-table-column :label="t('MenuManagement.Menu:Router')" prop="router" width="full"
                sortable></el-table-column>
            <el-table-column :label="t('MenuManagement.Menu:Order')" prop="order" width="100" align="right"
                sortable></el-table-column>
            <CheckColumn :label="t('MenuManagement.Menu:IsEnabled')" prop="isEnabled" width="100"
                :filterText="filterText" :checkChange="checkChange"></CheckColumn>
            <ActionColumn width="140" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>
    </div>

    <MenuForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId" />

</template>


<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import { ref, watch } from 'vue';
import { useI18n, useRepository, useTree } from '~/composables';
import { MenuType } from '~/repositories';
import CheckColumn from '../../table/CheckColumn.vue';
import ActionColumn from '../../table/ActionColumn.vue';
import Select from '../../forms/Select.vue';
import MenuForm from './MenuForm.vue';
import TreeColumn from '~/components/table/TreeColumn.vue';

const groupName = ref('');
const permissionVisible = ref(false);
const providerKey = ref('');
const api = useRepository('menu');
const { t } = useI18n();

const openPermissionWindow = (row: MenuType) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.name;
    permissionVisible.value = true;
}


const {
    data, dialogVisible, currentEntityId,
    filterText, refresh,
    create, update, remove, checkChange, filter, 
    sortChange, expandNode } = useTree<MenuType>(api);


const toolbarButtons = {
    //refresh: { action: api.load, icon: 'fa fa-refresh text-primary', title: 'Components.Refresh', order: 450, visible: true },
    create: { action: create, visible: api.canCreate },
}


watch(groupName, () => {
    api.search('groupName', groupName.value, true);
})




const tableButtons = {
    refresh: { action: refresh, icon: 'fa fa-refresh text-primary', title: 'Components.Refresh', order: 450, visible: true },
    detail:{ visible: false},
    edit: { action: update, disabled:(row: MenuType) => row.isStatic, visible: api.canUpdate },
    delete: { action: remove, disabled:(row: MenuType) => row.isStatic, visible: api.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', visible:api.canUpdate },
    global: { action: openPermissionWindow, icon: 'fa fa-globe', title: 'AbpIdentity.Permissions', order: 400, type: 'primary', visible:api.canUpdate },
}

//arrows-up-down-left-right

</script>