<template>
    <el-drawer v-bind="$attrs" :title="drawerTitle" direction="rtl" @open="onOpen">
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item label="ID">{{ data?.id }}</el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:UserName')" > {{ data?.userName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:Name')" > {{ data?.name || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:Surname')" > {{ data?.surname || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:Email')" > {{ data?.email || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:PhoneNumber')" > {{ data?.phoneNumber || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:IsActive')" > 
                <CheckStatus :value="(data?.isActive as boolean)" />
            </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.DisplayName:LockoutEnabled')" > 
                <CheckStatus :value="(data?.lockoutEnabled as boolean)" ></CheckStatus>
            </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.Locked')" > 
                {{ formatLockoutDate(data?.lockoutEnd as string) }}
            </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.CreationTime')" > {{ formatDate(data?.creationTime as string, 'yyyy-MM-dd HH:mm:ss') }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpIdentity.Roles')" > 
                <List :data="roles"></List>
            </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import {  UserType } from '~/repositories';
import { ref } from 'vue';
import { formatDate, i18n, isEmpty } from '~/libs';
import { useI18n, useRepository } from '~/composables';
import CheckStatus from '../icons/CheckStatus.vue';
import List from '../lists/List.vue';

const props = defineProps({
    entityId: {
        type: String,
        required: true
    }
})

const drawerTitle = ref('');
const data = ref<UserType>(null as any);
const roles = ref<string[]>([]);
const userApi = useRepository('user');
const {t} = useI18n();
const formatLockoutDate = (date: string | null) => {
    if (!date) {
        return i18n.get('AbpIdentity.NotLocked');
    }
    return formatDate(date, 'yyyy-MM-dd HH:mm:ss')
};

const onOpen = () => {
    if(isEmpty(props.entityId)) return;
    userApi.getEntity(props.entityId).then((res:any) => {
        drawerTitle.value = t.value('AbpIdentity.Details') + ' -' + res.userName;
        data.value = res;
    });
    userApi.getRoles(props.entityId).then((res:any) => {
        roles.value = res.items.map((role: any) => role.name);
    });
};


</script>

