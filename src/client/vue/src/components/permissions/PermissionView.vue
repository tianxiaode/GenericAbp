<template>
    <el-dialog :title="title" width="50%" destroy-on-close v-model="internalVisible">
        <form>
            <div v-for="group in permissions.groups">
                <h3>{{ group.displayName }}</h3>
                <div v-for="groupPermission in group.permissions.filter(m => m.parentName == null)">
                    <el-checkbox v-model="groupPermission.value" :checked="groupPermission.isGranted"
                        :true-value="groupPermission.name"
                        @change="(value: string) => groupPermissionChange(value, group, groupPermission)">
                        <h4>{{ groupPermission.displayName }}</h4>
                    </el-checkbox>
                    <div class="grid cols-5 ml-6">
                        <el-checkbox
                            v-for="childPermission in group.permissions.filter(m => m.parentName == groupPermission.name)"
                            v-model="childPermission.value" :checked="childPermission.isGranted"
                            :true-value="childPermission.name"
                            @change="(value: string) => childPermissionChange(value, group, childPermission)">
                            {{ childPermission.displayName }}
                        </el-checkbox>
                    </div>
                </div>
            </div>
        </form>
        <template #footer>
            <div class="spacer"></div>
            <el-button @click="handleClose">取消</el-button>
            <el-button type="primary" @click="handleSave">保存</el-button>
        </template>
    </el-dialog>
</template>
<script setup lang="ts">
import { toast, PermissionInterface, PermissionGroupInterface, PermissionGroupItemInterface, PermissionUpdateInterface, permission } from '~/libs';
import { ref, onMounted, watch } from 'vue'

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

const title = ref(`权限设置 - ${props.providerKey}`);
const internalVisible = ref(props.isVisible);
const permissions = ref({} as PermissionInterface);

const emit = defineEmits(['update:isVisible']);

// 用于监听属性变化
watch(() => props.isVisible, (newValue) => {
    internalVisible.value = newValue;
});

const handleClose = () => {
    internalVisible.value = false;
    emit('update:isVisible', false);
}

const groupPermissionChange = (value: string, group: PermissionGroupInterface, permission: PermissionGroupItemInterface) => {
    permission.value = value === permission.name ? value as any : '';
    permission.isGranted = value === permission.name;
    group.permissions.filter((m: any) => m.parentName == permission.name).forEach((childPermission: any) => {
        childPermission.value = permission.isGranted ? childPermission.name as any : '';
        childPermission.isGranted = permission.isGranted;
    })
}

const childPermissionChange = (value: string, group: PermissionGroupInterface, permission: PermissionGroupItemInterface) => {
    const parentPermission = group.permissions.find((m: any) => m.name == permission.parentName) as PermissionGroupItemInterface;
    permission.value = value;
    permission.isGranted = value === permission.name;
    const isAllGranted = group.permissions.filter((m: any) => m.parentName == parentPermission.name).every(m => m.isGranted);
    parentPermission.value = isAllGranted ? parentPermission.name as any : '';
    parentPermission.isGranted = isAllGranted;
};

const handleSave = () => {
    const newPermissions: PermissionUpdateInterface[] = [];
    permissions.value.groups.forEach((group: any) => {
        group.permissions.forEach((permission: any) => {
            newPermissions.push({
                name: permission.name,
                isGranted: permission.isGranted,
            });
        });
    });
    permission.update(props.providerName, props.providerKey, newPermissions).then(() => {
        toast.success('权限设置保存成功');
        handleClose();
    });
}

onMounted(() => {
    permission.get(props.providerName, props.providerKey).then((res: any) => {
        permissions.value = res;
    });
})

</script>
