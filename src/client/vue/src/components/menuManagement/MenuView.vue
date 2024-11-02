<template>
    <div class="flex flex-col gap-2">
        <!-- 顶部工具栏 -->
        <ActionToolbar :title="t('MenuManagement.Menus')" @filter="filter" :buttons="toolbarButtons" />
        <el-table ref="tableRef" :data="data" stripe border style="width: 100%" @sort-change="sortChange"
            :default-sort="{ prop: 'order', order: 'ascending' }" row-key="id"            
            lazy
            :load="isLazy ? loadNode : null"
            :default-expand-all="!isLazy"            
            :tree-props="isLazy ? { children: 'children', hasChildren: 'hasChildren' } : { children: 'children' }"
            >
            <HighlightColumn :label="t('MenuManagement.Menu:Name')" prop="name" width="full" sortable
                :filterText="filterText" />
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
            <ActionColumn width="120" align="center" :buttons="tableButtons"></ActionColumn>
        </el-table>
    </div>

</template>


<script setup lang="ts">
import ActionToolbar from '../toolbars/ActionToolbar.vue';
import { ref, watch } from 'vue';
import { useI18n, useRepository, useTable } from '~/composables';
import { MenuType } from '~/repositories';
import HighlightColumn from '../table/HighlightColumn.vue';
import CheckColumn from '../table/CheckColumn.vue';
import ActionColumn from '../table/ActionColumn.vue';
import PermissionView from '../permissions/PermissionView.vue';
import { isEmpty } from '~/libs';
import { TreeNode } from 'element-plus';

const tableRef = ref<any>();

const permissionVisible = ref(false);
const providerKey = ref('');
const isLazy = ref(true);
const api = useRepository('menu', true, {
    useCache: false,
    afterLoad(data: MenuType[]) {
        if (!isEmpty(filterText.value)) {
            data = getChildren(data, null);
        } else {
            data.forEach((item: MenuType) => {
                item.hasChildren = !item.leaf;
            });
        }
        return data;

    }
});
const { t } = useI18n();

const openPermissionWindow = (row: MenuType) => {
    // TODO: 打开权限定义窗口
    providerKey.value = row.name;
    permissionVisible.value = true;
}


const {
    data, dialogVisible, currentEntityId,
    filterText,
    create, update, remove, filter, checkChange,
    sortChange } = useTable<MenuType>(api);

const toolbarButtons = {
    create: { action: create, isVisible: api.canCreate },
}

watch(filterText, () => {
    isLazy.value = isEmpty(filterText.value);
})

const loadNode = (row: MenuType, _: TreeNode, resolve: any) => {
    if (!isEmpty(filterText.value)) {
        resolve([]);
        return;
    };
    const parentId = row.id;
    api.getList({ parentId }).then((res: any) => {
        res.forEach((item: any) => {
            item.hasChildren = !item.leaf;
        })
        resolve(res);
    })
}

const getChildren = (data: any, id: any) => {
    let children = [...data.filter((item: any) => item.parentId === id)];
    children.forEach((item: any) => {
        console.log('getChildren', item.id, item.name)
        item.children = getChildren(data, item.id);
        tableRef.value.updateKeyChildren(item.id, item.children);
    })
    return children;
}

const updateKeyChildren = (data:any) => {
    if(!data || data.length === 0) return;
    data.forEach((item: any) => {
        tableRef.value.updateKeyChildren(item.id, []);   
    });
}

const tableButtons = {
    edit: { action: update, isVisible: api.canUpdate },
    delete: { action: remove, isVisible: api.canDelete },
    permission: { action: openPermissionWindow, icon: 'fa fa-lock', title: 'AbpIdentity.Permissions', order: 300, type: 'primary', isVisible: () => api.canUpdate },
    global: { action: openPermissionWindow, icon: 'fa fa-globe', title: 'AbpIdentity.Permissions', order: 400, type: 'primary', isVisible: () => api.canUpdate },
}


</script>