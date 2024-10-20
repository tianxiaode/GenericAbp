<template>
    <el-dialog width="50%" destroy-on-close v-model="internalVisible">
        <template #header>
            {{ t('AbpIdentity.Permissions') }} - {{ providerKey }}
        </template>
        <form>
            <el-container>
                <aside>
                    <el-menu :default-active="currentGroup.name">
                        <el-menu-item v-for="group in groups" :index="group.name"
                            @click="() => handleSelect(group)">
                            {{ group.displayName }}
                        </el-menu-item>
                    </el-menu>
                </aside>
                <main>
                    <div class="p-2" v-for="groupPermission in currentGroup.permissions?.filter(m => m.parentName == null)">
                        <el-checkbox v-model="groupPermission.isGranted"
                            @change="()=>groupPermissionChange(groupPermission)"
                        >
                            <h4>{{ groupPermission.displayName }}</h4>
                        </el-checkbox>
                        <div class="grid cols-1 ml-6">
                            <el-checkbox v-for="childPermission in currentGroup.permissions?.filter(m => m.parentName == groupPermission.name)" 
                                v-model="childPermission.isGranted"
                                @change="()=>childPermissionChange(childPermission)"
                                >
                                {{ childPermission.displayName }}
                            </el-checkbox>
                        </div>

                    </div>

                </main>
            </el-container>
        </form>
        <template #footer>
            <div class="spacer"></div>
            <el-button @click="handleClose">{{ t('Components.Cancel') }}</el-button>
            <el-button type="primary" @click="handleSave">{{ t('Components.Save') }}</el-button>
        </template>
    </el-dialog>
</template>
<script setup lang="ts">
import {  PermissionGroupInterface, PermissionGroupItemInterface, PermissionUpdateInterface, permission, i18n } from '~/libs';
import { ref, onMounted, watch, reactive } from 'vue'
import { ElMessage } from 'element-plus';
import { useI18n } from '~/composables';

const props = defineProps({
    isVisible: {
        type: Boolean,
        default: false
    },
    providerName: {
        type: String,
        required: true
    },
    providerKey: {
        type: String,
        required: true
    }
})

const { t } = useI18n();
const internalVisible = ref(props.isVisible);
const groups = ref([] as PermissionGroupInterface[]);
const currentGroup = ref({} as PermissionGroupInterface);
const emit = defineEmits(['update:isVisible']);

// 用于监听属性变化
watch(() => props.isVisible, (newValue) => {
    internalVisible.value = newValue;
});

const handleClose = () => {
    internalVisible.value = false;
    emit('update:isVisible', false);
}

const groupPermissionChange = (permission: PermissionGroupItemInterface) => {
    const groupName = permission.name.split('.')[0];
    const group = groups.value.find((m: any) => m.name == groupName) as PermissionGroupInterface;
    group.permissions.filter((m: any) => m.parentName == permission.name).forEach((childPermission: any) => {
        childPermission.isGranted = permission.isGranted;
    })
}

const childPermissionChange = (permission: PermissionGroupItemInterface) => {
    const groupName = permission.name.split('.')[0];
    const group = groups.value.find((m: any) => m.name == groupName) as PermissionGroupInterface;
    const parentPermission = group.permissions.find((m: any) => m.name == permission.parentName) as PermissionGroupItemInterface;
    const isAllGranted = group.permissions.filter((m: any) => m.parentName == parentPermission.name).every(m => m.isGranted);
    parentPermission.isGranted = isAllGranted;
};


const handleSave = () => {
    const newPermissions: PermissionUpdateInterface[] = [];
    groups.value.forEach((group: any) => {
        group.permissions.forEach((permission: any) => {
            newPermissions.push({
                name: permission.name,
                isGranted: permission.isGranted,
            });
        });
    });
    permission.update(props.providerName, props.providerKey, newPermissions).then(() => {
        ElMessage.success(i18n.get('Message.SaveSuccess'));
        handleClose();
    });
}

const handleSelect = (group: PermissionGroupInterface) => {
    currentGroup.value = group;
}


onMounted(() => {
    permission.get(props.providerName, props.providerKey).then((res: any) => {
        groups.value = res.groups.sort((a: any, b: any) => a.displayName.localeCompare(b.displayName));
        currentGroup.value = res.groups[0];
    });
})

</script>
