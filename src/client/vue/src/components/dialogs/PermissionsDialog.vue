<template>
    <BaseDialog v-bind="$attrs" ref="dialogRef">
        <el-container style="min-height: 580px;" v-if="groups.length > 0">
            <aside style="min-height: 100%;">
                <el-menu :default-active="currentGroup.name" class="h-full">
                    <el-menu-item v-for="group in groups" :index="group.name" @click="() => handleSelect(group)">
                        {{ group.displayName }}
                    </el-menu-item>
                </el-menu>
            </aside>
            <main class="flex-1 p-2">
                <div class="w-full box-border border-b">
                    <el-checkbox v-model="selectAllInGroup" :indeterminate="isSelectAllInGroupIndeterminate"
                        @change="selectAllInGroupChange">
                        {{ t('AbpPermissionManagement.SelectAllInThisTab') }}
                    </el-checkbox>
                </div>
                <div v-for="groupPermission in currentGroup.permissions?.filter(m => m.parentName == null)">
                    <el-checkbox v-model="groupPermission.isGranted"
                        @change="() => handleGroupPermissionChange(groupPermission)">
                        <h4>{{ groupPermission.displayName }}</h4>
                    </el-checkbox>
                    <div class="grid cols-1 ml-6">
                        <div
                            v-for="childPermission in currentGroup.permissions?.filter(m => m.parentName == groupPermission.name)">
                            <el-checkbox v-model="childPermission.isGranted"
                                @change="() => handleChildPermissionChange(childPermission)">
                                {{ childPermission.displayName }}
                            </el-checkbox>
                            <div class="grid cols-1 ml-6">
                                <el-checkbox
                                    v-for="subChildPermission in currentGroup.permissions?.filter(m => m.parentName == childPermission.name)"
                                    :key="subChildPermission.name" v-model="subChildPermission.isGranted"
                                    @change="() => handleChildPermissionChange(subChildPermission)">
                                    {{ subChildPermission.displayName }}
                                </el-checkbox>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </el-container>
        <template #dialog-actions>
            <el-checkbox v-model="grantAll" :indeterminate="isGrantAllIndeterminate" @change="grantAllChange"
                style="order: 300;">
                {{ t('AbpPermissionManagement.SelectAllInAllTabs') }}
            </el-checkbox>
        </template>
    </BaseDialog>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
import BaseDialog from './BaseDialog.vue';
import { PermissionGroupInterface, PermissionGroupItemInterface } from '~/libs';
import { onMounted, ref } from 'vue';


const dialogRef = defineModel<any>('dialogRef');
const { t } = useI18n();
const grantAll = ref(false);
const isGrantAllIndeterminate = ref(false);
const selectAllInGroup = ref(false);
const isSelectAllInGroupIndeterminate = ref(false);
const groups = defineModel<PermissionGroupInterface[]>({ default: [] });
const currentGroup = ref({} as PermissionGroupInterface);

const updateChildPermissions = (permission: PermissionGroupItemInterface, isGranted: boolean) => {
    const childPermissions = currentGroup.value.permissions.filter(m => m.parentName?.startsWith(permission.name as string));
    childPermissions.forEach(p => p.isGranted = isGranted);
};

const handleGroupPermissionChange = (permission: PermissionGroupItemInterface) => {
    updateChildPermissions(permission, permission.isGranted as boolean);
    checkAllStatus();
};

const handleChildPermissionChange = (permission: PermissionGroupItemInterface) => {
    // 找到当前子权限的父权限
    const parentPermission = currentGroup.value.permissions.find(m => m.parentName === null && permission.name.startsWith(m.name as string));

    if (parentPermission) {
        // 更新父权限下的直接子权限以及进一步的子权限
        const allChildPermissions = currentGroup.value.permissions
            .filter(m => m.parentName?.startsWith(parentPermission.name as string)); // 包含所有层级的子权限

        // 检查是否所有子权限都被授予
        parentPermission.isGranted = allChildPermissions.every(m => m.isGranted); // 更新父权限状态
    }


    checkAllStatus(); // 更新总状态
};

const checkAllStatus = () => {
    const allPermissions = groups.value.flatMap(group => group.permissions);
    const allGranted = allPermissions.every(permission => permission.isGranted);
    const hasGrant = allPermissions.some(permission => permission.isGranted);

    isGrantAllIndeterminate.value = hasGrant && !allGranted;
    grantAll.value = allGranted;

    const groupPermissions = currentGroup.value.permissions;
    const groupAllGranted = groupPermissions.every(permission => permission.isGranted);
    const groupHasGrant = groupPermissions.some(permission => permission.isGranted);

    isSelectAllInGroupIndeterminate.value = groupHasGrant && !groupAllGranted;
    selectAllInGroup.value = groupAllGranted;
};

const handleSelect = (group: PermissionGroupInterface) => {
    currentGroup.value = group;
    checkAllStatus();
};

const grantAllChange = () => {
    groups.value.forEach(group => {
        group.permissions.forEach(permission => {
            permission.isGranted = grantAll.value;
        });
    });
    checkAllStatus();
};

const selectAllInGroupChange = () => {
    currentGroup.value.permissions.forEach(permission => {
        permission.isGranted = selectAllInGroup.value;
    });
    checkAllStatus();
};

onMounted(()=>{
    currentGroup.value = groups.value[0];
    checkAllStatus();
})

</script>
