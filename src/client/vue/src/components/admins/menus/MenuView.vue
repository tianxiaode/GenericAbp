<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('MenuManagement.Menus')" @filter="filter" :buttons="toolbarButtons">
        </ActionToolbar>
        <el-table ref="tableRef" :data="data" stripe border style="width: 100%" :row-key="api.idFieldName"
            :highlight-current-row="true">
            <TreeColumn prop="name" :label="t(getLabel('Name'))" width="full" :filterText="filterText"
                :order="sorts.name" :sort-change="sortChange" :expand="expandNode">
            </TreeColumn>
            <CustomSortColumn :label="t(getLabel('Icon'))" prop="icon" width="100" :sort-change="sortChange"
                :order="sorts.icon">
                <template #default="{ row }">
                    <i v-if="row.icon" :class="row.icon"></i>
                    <span v-else>-</span>
                </template>
            </CustomSortColumn>
            <CustomSortColumn :label="t(getLabel('Router'))" prop="router" width="full" :is-highlight="true"
                :order="sorts.router" :filter="filterText" :sort-change="sortChange">
            </CustomSortColumn>
            <CustomSortColumn :label="t(getLabel('Order'))" prop="order" width="100" align="right"
                :order="sorts.order" :sort-change="sortChange">
            </CustomSortColumn>
            <CheckColumn :label="t(getLabel('IsEnabled'))" prop="isEnabled" width="100"
                :filterText="filterText" :checkChange="checkChange"></CheckColumn>
            <ActionColumn width="200" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>
    </div>

    <MenuForm v-if="dialogVisible" v-model="dialogVisible" v-model:entity-id="currentEntityId"
        :parent="currentParent" />
    <Detail v-if="detailVisible" :title="detailTitle" :data="detailData" :row-items="rowItems" v-model="detailVisible">
    </Detail>
    <MultilingualDialog v-bind="multilingualDialogProps" v-if="multilingualDialogVisible"
        v-model:dialog-ref="multilingualDialogRef"
        v-model="multilingualDialogData">
    </MultilingualDialog>
</template>


<script setup lang="ts">
import ActionToolbar from '../../toolbars/ActionToolbar.vue';
import { ref, watch } from 'vue';
import { useI18n, useRepository, useTree, useDetail, useMultilingualDialog } from '~/composables';
import { MenuType } from '~/repositories';
import CheckColumn from '../../table/CheckColumn.vue';
import ActionColumn from '../../table/ActionColumn.vue';
import MenuForm from './MenuForm.vue';
import TreeColumn from '~/components/table/TreeColumn.vue';
import CustomSortColumn from '~/components/table/CustomSortColumn.vue';
import Detail from '../../Detail.vue';
import MultilingualDialog from '~/components/dialogs/MultilingualDialog.vue';

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
    data, dialogVisible, currentEntityId, tableRef,
    filterText, sorts, buttons, currentParent,
    create, update, remove, checkChange, filter, getLabel,
    sortChange, expandNode } = useTree<MenuType>(api, { order: 'ascending' });


const toolbarButtons = {
    //refresh: { action: api.load, icon: 'fa fa-refresh text-primary', title: 'Components.Refresh', order: 450, visible: true },
    create: { action: create, visible: api.canCreate },
}


watch(groupName, () => {
    api.search('groupName', groupName.value, true);
})

const { detailVisible, detailData, detailTitle, showDetails, rowItems } = useDetail(api, [
    { field: 'name' },
    { field: 'parent', convert: (_: any, data: MenuType) => data.parent ? data.parent.name : '-' },
    { field: 'icon', render: (value: any) => value ? `<i class="${value}"></i>` : '-' },
    { field: 'router' },
    { field: 'order' },
    { field: 'isEnabled', type: 'boolean' },
    { field: 'isStatic', type: 'boolean' },
    { field: 'multilingual', type: 'multilingual' },
    { field: 'permissions', type: 'list' }
])

const { multilingualDialogRef, multilingualDialogVisible, multilingualDialogProps, multilingualDialogData, multilingualShowDialog } = useMultilingualDialog(api);

const tableButtons = {
    ...buttons,
    detail: { action: showDetails },
    edit: { action: update, disabled: (row: MenuType) => row.isStatic, visible: api.canUpdate },
    delete: { action: remove, disabled: (row: MenuType) => row.isStatic, visible: api.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', visible: api.canUpdate },
    global: { action: multilingualShowDialog, icon: 'fa fa-globe', title: 'AbpIdentity.Permissions', order: 400, type: 'primary', visible: api.canUpdate },
}

//arrows-up-down-left-right

</script>