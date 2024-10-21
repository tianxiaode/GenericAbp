<template>
    <el-dialog width="50%" destroy-on-close v-model="internalVisible">
        <template #header>
            {{ t('AbpIdentity.Permissions') }} - {{ title }}
        </template>
        <form>
            <el-container style="min-height: 500px;">
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
                    <div v-for="groupPermission in currentGroup.permissions?.filter(m => m.parentName == null)" >
                        <el-checkbox v-model="groupPermission.isGranted" @change="() => handleGroupPermissionChange(groupPermission)">
                            <h4>{{ groupPermission.displayName }}</h4>
                        </el-checkbox>
                        <div class="grid cols-1 ml-6">
                            <div v-for="childPermission in currentGroup.permissions?.filter(m => m.parentName == groupPermission.name)" >
                                <el-checkbox v-model="childPermission.isGranted" @change="() => handleChildPermissionChange(childPermission)">
                                    {{ childPermission.displayName }}
                                </el-checkbox>
                                <div class="grid cols-1 ml-6">
                                    <el-checkbox
                                        v-for="subChildPermission in currentGroup.permissions?.filter(m => m.parentName == childPermission.name)" :key="subChildPermission.name"
                                        v-model="subChildPermission.isGranted"
                                        @change="() => handleChildPermissionChange(subChildPermission)">
                                        {{ subChildPermission.displayName }}
                                    </el-checkbox>
                                </div>
                            </div>
                        </div>
                    </div>
                </main>
            </el-container>
        </form>
        <template #footer>
            <el-checkbox v-model="grantAll" :indeterminate="isGrantAllIndeterminate" @change="grantAllChange">
                {{ t('AbpPermissionManagement.SelectAllInAllTabs') }}
            </el-checkbox>
            <div class="spacer"></div>
            <el-button @click="handleClose">{{ t('Components.Cancel') }}</el-button>
            <el-button type="primary" @click="handleSave">{{ t('Components.Save') }}</el-button>
        </template>
    </el-dialog>
</template>
<script setup lang="ts">
import { PermissionGroupInterface, PermissionGroupItemInterface, PermissionUpdateInterface, permission, i18n } from '~/libs';
import { ref, onMounted, watch } from 'vue';
import { ElMessage } from 'element-plus';
import { useI18n } from '~/composables';

const props = defineProps({
    isVisible: { type: Boolean, default: false },
    providerName: { type: String, required: true },
    providerKey: { type: String, required: true },
    title: { type: String, required: true },
});

const { t } = useI18n();
const grantAll = ref(false);
const isGrantAllIndeterminate = ref(false);
const selectAllInGroup = ref(false);
const isSelectAllInGroupIndeterminate = ref(false);
const internalVisible = ref(props.isVisible);
const groups = ref([] as PermissionGroupInterface[]);
const currentGroup = ref({} as PermissionGroupInterface);
const emit = defineEmits(['update:isVisible']);

watch(() => props.isVisible, (newValue) => {
    internalVisible.value = newValue;
});

const handleClose = () => {
    internalVisible.value = false;
    emit('update:isVisible', false);
};

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

    console.log(parentPermission);
    if (parentPermission) {
        // 更新父权限下的直接子权限以及进一步的子权限
        const allChildPermissions = currentGroup.value.permissions
            .filter(m => m.parentName?.startsWith(parentPermission.name as string)); // 包含所有层级的子权限

        console.log('allChildPermissions',allChildPermissions);
        // 检查是否所有子权限都被授予
        parentPermission.isGranted = allChildPermissions.every(m => m.isGranted); // 更新父权限状态
    }


    checkAllStatus(); // 更新总状态
};


const handleSave = async () => {
    try {
        const newPermissions: PermissionUpdateInterface[] = groups.value.flatMap(group => 
            group.permissions.map(permission => ({ name: permission.name, isGranted: permission.isGranted }))
        );
        await permission.update(props.providerName, props.providerKey, newPermissions);
        ElMessage.success(i18n.get('Message.SaveSuccess'));
        handleClose();
    } catch (error) {
        ElMessage.error(i18n.get('Message.SaveFailed'));
    }
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

onMounted(async () => {
    try {
        const res = await permission.get(props.providerName, props.providerKey);
        groups.value = res.groups.sort((a: any, b: any) => a.displayName.localeCompare(b.displayName));
        currentGroup.value = groups.value[0];
    } catch (error) {
        ElMessage.error(i18n.get('Message.LoadFailed'));
    }
});
</script>
