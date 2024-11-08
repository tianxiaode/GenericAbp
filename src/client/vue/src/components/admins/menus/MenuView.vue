<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('MenuManagement.Menus')" @filter="filter" :buttons="toolbarButtons">
        </ActionToolbar>
        <el-table :data="data" stripe border style="width: 100%" row-key="id">
            <TreeColumn prop="name" :label="t('MenuManagement.Menu:Name')" width="full" :filterText="filterText"
                :order="sorts.name"
                :sort-change="sortChange"
                :expand="expandNode">
            </TreeColumn>
            <CustomSortColumn :label="t('MenuManagement.Menu:Icon')" prop="icon" width="100" :sort-change="sortChange"
                :order="sorts.icon"
            >
                <template #default="{ row }">
                    <i v-if="row.icon" :class="row.icon"></i>
                    <span v-else>-</span>
                </template>
            </CustomSortColumn>
            <CustomSortColumn :label="t('MenuManagement.Menu:Router')" prop="router" width="full" :is-highlight="true"
                :order="sorts.router"
                :filter="filterText" :sort-change="sortChange">
            </CustomSortColumn>
            <CustomSortColumn :label="t('MenuManagement.Menu:Order')" prop="order" width="100" align="right"
                :order="sorts.order" :sort-change="sortChange">
            </CustomSortColumn>
            <CheckColumn :label="t('MenuManagement.Menu:IsEnabled')" prop="isEnabled" width="100"
                :filterText="filterText" :checkChange="checkChange"></CheckColumn>
            <ActionColumn width="160" align="center" :buttons="tableButtons"></ActionColumn>
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
import MenuForm from './MenuForm.vue';
import TreeColumn from '~/components/table/TreeColumn.vue';
import CustomSortColumn from '~/components/table/CustomSortColumn.vue';

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
    filterText, sorts, refreshButton,
    create, update, remove, checkChange, filter,
    sortChange, expandNode } = useTree<MenuType>(api,{ order: 'ascending'});


const toolbarButtons = {
    //refresh: { action: api.load, icon: 'fa fa-refresh text-primary', title: 'Components.Refresh', order: 450, visible: true },
    create: { action: create, visible: api.canCreate },
}


watch(groupName, () => {
    api.search('groupName', groupName.value, true);
})




const tableButtons = {
    refresh: refreshButton(),
    detail: { visible: false },
    edit: { action: update, disabled: (row: MenuType) => row.isStatic, visible: api.canUpdate },
    delete: { action: remove, disabled: (row: MenuType) => row.isStatic, visible: api.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', visible: api.canUpdate },
    global: { action: openPermissionWindow, icon: 'fa fa-globe', title: 'AbpIdentity.Permissions', order: 400, type: 'primary', visible: api.canUpdate },
}

//arrows-up-down-left-right

</script>