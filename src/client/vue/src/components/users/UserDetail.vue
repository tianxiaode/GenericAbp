<template>
    <el-drawer v-bind="$props" :title="drawerTitle" direction="rtl">
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item label="ID">{{ data?.id }}</el-descriptions-item>
            <el-descriptions-item label="用户名" > {{ data?.userName || '-' }} </el-descriptions-item>
            <el-descriptions-item label="姓名" > {{ data?.surname || '-' }} </el-descriptions-item>
            <el-descriptions-item label="电子邮箱" > {{ data?.email || '-' }} </el-descriptions-item>
            <el-descriptions-item label="电话" > {{ data?.phoneNumber || '-' }} </el-descriptions-item>
            <el-descriptions-item label="启用" > 
                <font-awesome-icon size="lg" :icon="data?.isActive ? 'square-check' : 'square'" :class="data?.isActive ? 'primary' : '' " /> 
            </el-descriptions-item>
            <el-descriptions-item label="账户锁定" > 
                <font-awesome-icon size="lg" :icon="data?.lockoutEnabled ? 'square-check' : 'square'" :class="data?.isActive ? 'primary' : '' " /> 
            </el-descriptions-item>
            <el-descriptions-item label="已锁定" > 
                {{ formatLockoutDate(data?.lockoutEnd as string) }}
            </el-descriptions-item>
            <el-descriptions-item label="创建时间" > {{ formatDate(data?.creationTime as string, 'yyyy-MM-dd HH:mm:ss') }} </el-descriptions-item>
            <el-descriptions-item label="角色" > {{ roles.join(', ') }} </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import { userApi, UserType } from '../../repositories';
import { watch, ref } from 'vue';
import { formatDate } from '../../libs';

const props = defineProps({
    entityId: {
        type: String,
        required: true
    }
})

const drawerTitle = ref('');
const data = ref<UserType>(null as any);
const roles = ref<string[]>([]);

const formatLockoutDate = (date: string | null) => {
    if (!date) {
        return '未锁定';
    }
    return formatDate(date, 'yyyy-MM-dd HH:mm:ss')
};



watch (() => props.entityId, async () => {
    userApi.getSingle(props.entityId).then((res) => {
        drawerTitle.value = '用户详情 -' + res.userName;
        data.value = res;
    });
    userApi.getRoles(props.entityId).then((res) => {
        roles.value = res.items.map((role: any) => role.name);
    });
});

</script>

